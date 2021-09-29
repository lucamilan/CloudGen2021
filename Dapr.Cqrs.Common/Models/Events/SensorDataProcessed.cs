using System;
using System.Text.Json.Serialization;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Common.Notification;

namespace Dapr.Cqrs.Common.Models.Events
{
    public class SensorDataProcessed
    {
        public Guid EventId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StorageType StorageType { get; set; }

        public NotificationType ConvertToNotificationType()
        {
            return StorageType switch
            {
                StorageType.Raw => NotificationType.DataRawProcessing,
                StorageType.Time => NotificationType.DataTimeProcessing,
                StorageType.Search => NotificationType.DataSearchProcessing,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString()
        {
            return $"{StorageType}@{EventId}";
        }
    }
}