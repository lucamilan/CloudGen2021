using System.Text.Json.Serialization;

namespace Dapr.Cqrs.Common.Notification
{
    public record NotificationData
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; init; }

        public string Message { get; init; }

        public override string ToString()
        {
            return $"{Type} {Message}";
        }
    }
}