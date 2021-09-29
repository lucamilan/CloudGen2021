using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.ConnectionStrings;
using Dapr.Cqrs.Processor.Time.Models;

namespace Dapr.Cqrs.Processor.Time.Services {
    public class TimeTablesManagement {
        private readonly TableServiceClient _tableServiceClient;

        public TimeTablesManagement (ConnectionStringsRegistry connectionStringsRegistry) {
            if (connectionStringsRegistry == null) throw new ArgumentNullException (nameof (connectionStringsRegistry));
            _tableServiceClient = new TableServiceClient (connectionStringsRegistry.GetAzureTables ());
        }

        public async Task CreateRecordForTimeAsync (Guid id, SensorData sensorData) {
            var tableForTimeRecords = await GetTableClientAsync (AzureTablesNames.TableOptimizedForQueryTimeBased);
            var entity = TimeRecordTableEntity.Create (id, sensorData);
            await tableForTimeRecords.AddEntityAsync (entity);
        }
        public async Task<IEnumerable<TimeAggregateTableEntity>> ProcessDataByIntervalsAsync (TableClient tableForTimeRecords) {
            var rows = new Dictionary<string, TimeAggregateTableEntity> ();

            foreach (var interval in TimeAggregateTableEntity.Intervals) {
                foreach (var plant in SensorDataLookup.Plants.Keys) {
                    foreach (var location in SensorDataLookup.Locations.Keys) {
                        var filter = TimeRecordTableEntity.BuildQueryFilterForTimeScan (interval, plant, location);
                        var tableEntities = await tableForTimeRecords.QueryAsync<TimeRecordTableEntity> (filter).ToListAsync ();
                        var entitiesMap = tableEntities.GroupBy (_ => _.Tag).ToDictionary (_ => _.Key, _ => _.ToArray ());

                        foreach (var tag in SensorDataLookup.Tags.Keys) {
                            var entityKey = $"{location}{tag}{plant}".ToUpperInvariant ();

                            if (!rows.TryGetValue (entityKey, out var entity)) {
                                entity = new TimeAggregateTableEntity {
                                    PartitionKey = $"{location}{tag}".ToUpperInvariant (),
                                    RowKey = plant.ToUpperInvariant (),
                                    PlantLabel = SensorDataLookup.Plants[plant],
                                    LocationLabel = SensorDataLookup.Locations[location],
                                    TagLabel = SensorDataLookup.Tags[tag],
                                };

                                rows.TryAdd (entityKey, entity); //32 = 4 plant * 4 location * 2 tag
                            }

                            var records = entitiesMap.ContainsKey (tag) ? entitiesMap[tag] : Array.Empty<TimeRecordTableEntity> ();

                            TimeAggregateTableEntity.Update (interval, entity, records);

                            //Console.WriteLine($"{entityKey} {interval} FILTER: {filter} TE: {tableEntities.Count} RE: {records.Length}");
                        }
                    }
                }
            }

            return rows.Values;
        }

        public async Task<TableClient> GetTableClientAsync (string tableName, CancellationToken cancellationToken = default) {
            await _tableServiceClient.CreateTableIfNotExistsAsync (tableName, cancellationToken);
            return _tableServiceClient.GetTableClient (tableName);
        }
    }
}