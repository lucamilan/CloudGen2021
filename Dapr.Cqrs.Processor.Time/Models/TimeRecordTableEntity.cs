using System;
using Azure;
using Azure.Data.Tables;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Processor.Time.Models
{
    public class TimeRecordTableEntity : ITableEntity
    {
        public static string BuildQueryFilterForTimeScan(int intervalInSeconds, string plant, string location)
        {
            var partitionLowerBound =$"{plant}/{location}/";
            var partitionUpperBound  = CreateUpperBoundString($"{plant}/{location}/");
            var rowKey = $"{DateTime.MaxValue.Ticks - DateTime.UtcNow.AddSeconds(-1 * intervalInSeconds).Ticks:D19}";
            var filter = $" (PartitionKey ge '{partitionLowerBound}' and PartitionKey le '{partitionUpperBound}') and '{rowKey}' ge RowKey";
            return filter;
        }

        private static string CreateUpperBoundString(string lowerBoundString)
        {
            return lowerBoundString.Substring(0, lowerBoundString.Length - 1) + (char)(lowerBoundString[^1] + 1);
        }

        public static TimeRecordTableEntity Create(Guid id, SensorData sensorData)
        {
            if (sensorData is null)
            {
                throw new ArgumentNullException(nameof(sensorData));
            }

            var rowKey = $"{DateTime.MaxValue.Ticks - sensorData.RecordedOn.ToUniversalTime().Ticks:D19}/{id:N}";
            //DateTime dt = new DateTime(DateTime.MaxValue.Ticks - Int64.Parse(invertedTicks));

            var data = new TimeRecordTableEntity
            {
                PartitionKey = $"{sensorData.Plant}/{sensorData.Location}/{sensorData.Tag}".ToUpperInvariant(),
                RowKey = rowKey,
                EventId = id,
                Value = sensorData.Value,
                Tag = sensorData.Tag,
                RecordedOn = sensorData.RecordedOn.ToUniversalTime()
            };

            return data;      
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public Guid EventId { get; set; }
        public double Value { get; set; }
        public string Tag { get; set; }
        public DateTime RecordedOn { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}