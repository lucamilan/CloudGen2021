using System;
using System.Collections.Generic;
using Azure.Data.Tables;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Api.Read.Mappers
{

    public static class RealtimeAggregatesMapper
    {
        public static AggregateDto Map(TableEntity entity)
        {
            var realtimeAggregate = new AggregateDto
            {
                PlantId = entity.RowKey,
                LocationId = entity.PartitionKey.Substring(0,3),
                TagId = entity.PartitionKey.Substring(3),

                PlantLabel = entity.GetString(nameof(AggregateDto.PlantLabel)),
                LocationLabel = entity.GetString(nameof(AggregateDto.LocationLabel)),
                TagLabel = entity.GetString(nameof(AggregateDto.TagLabel)),

                Timestamp = entity.GetDateTimeOffset(nameof(entity.Timestamp))
            };

            for (var i = 0; i < 4; i++)
            {
                var values = new AggregateValuesDto
                {
                    IntervalId = i.ToString(),
                    Count = entity.GetInt32($"Count{i}").GetValueOrDefault(),
                    Min = entity.GetDouble($"Min{i}").GetValueOrDefault(),
                    Max = entity.GetDouble($"Max{i}").GetValueOrDefault(),
                    Average = entity.GetDouble($"Average{i}").GetValueOrDefault(),
                    StandardDeviation = entity.GetDouble($"StdDev{i}").GetValueOrDefault(),
                };

                realtimeAggregate.Values.Add(values);
            }

            return realtimeAggregate;
        }

        internal static List<AggregateDto> Build()
        {
            var list = new List<AggregateDto>();

            foreach (var plantId in SensorDataLookup.Plants.Keys)
            {
                foreach (var locationId in SensorDataLookup.Locations.Keys)
                {
                    foreach (var tagId in SensorDataLookup.Tags.Keys)
                    {
                        var realtimeAggregate = new AggregateDto
                        {
                            PlantId = plantId,
                            LocationId = locationId,
                            TagId = tagId,
                            PlantLabel = SensorDataLookup.Plants[plantId],
                            LocationLabel = SensorDataLookup.Locations[locationId],
                            TagLabel = SensorDataLookup.Tags[tagId],
                            Timestamp = DateTimeOffset.Now
                        };

                        for (var i = 0; i < 4; i++)
                        {
                            realtimeAggregate.Values.Add(new AggregateValuesDto { IntervalId = i.ToString(), Max = 4, Min = 0, Count = i, Average = 1 + i, StandardDeviation = 1 + i });
                        }

                        list.Add(realtimeAggregate);
                    }
                }
            }

            return list;
        }

    }
}
