using System;
using System.Linq;
using Azure.Data.Tables;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Api.Read.Mappers
{
    public static class SensorDtoMapper
    {
        public static SensorDto Map(TableEntity entity)
        {
            var tokens = entity.PartitionKey.Split('/');
            var invertedTicks = entity.RowKey.Split('/').First();

            return new SensorDto
            {
                EventId = entity.GetGuid(nameof(SensorDto.EventId)).GetValueOrDefault(),
                PlantLabel = SensorDataLookup.Plants[tokens[0]],
                LocationLabel = SensorDataLookup.Locations[tokens[1]],
                TagLabel = SensorDataLookup.Tags[tokens[2]],
                Value = entity.GetDouble(nameof(SensorDto.Value)).GetValueOrDefault(),
                RecordedOn = new DateTime(DateTime.MaxValue.Ticks - Int64.Parse(invertedTicks))
            };
        }
    }
}
