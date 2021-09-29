using Azure.Data.Tables;
using Dapr.Cqrs.Api.Read.Mappers;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Core.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Core;

namespace Dapr.Cqrs.Api.Read.Services
{
    public class AzureTablesManagement
    {
        private readonly TableServiceClient _tableServiceClient;

        public AzureTablesManagement(ConnectionStringsRegistry connectionStringsRegistry)
        {
            if (connectionStringsRegistry == null) throw new ArgumentNullException(nameof(connectionStringsRegistry));
            _tableServiceClient = new TableServiceClient(connectionStringsRegistry.GetAzureTables());
        }

        public async Task<IEnumerable<AggregateDto>> GetRealtimeAggregatesAsync()
        {
            var table = await GetTableClientAsync(AzureTablesNames.MaterializedViewForRealtimeAggregates);

            var list = await table.QueryAsync<TableEntity>().ToListAsync();

            return list.Select(RealtimeAggregatesMapper.Map).ToList();
        }

        public async Task<IEnumerable<SensorDto>> GetLastRecordsAsync(int numberOfSeconds, string plantKey)
        {
            var table = await GetTableClientAsync(AzureTablesNames.TableOptimizedForQueryTimeBased);

            var filter = MiscExtensions.BuildQueryFilterForTimeScan( $"{plantKey}/", numberOfSeconds );

            var list = await table.QueryAsync<TableEntity>(filter: filter).ToListAsync();

            return list.Select(SensorDtoMapper.Map).ToList();
        }
        public async Task<TableClient> GetTableClientAsync(string tableName, CancellationToken cancellationToken = default)
        {
            await _tableServiceClient.CreateTableIfNotExistsAsync(tableName, cancellationToken);
            return _tableServiceClient.GetTableClient(tableName);
        }
    }
}