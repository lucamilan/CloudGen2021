using Dapr.Cqrs.Common.Notification;

namespace Dapr.Cqrs.Common.Models.Read
{
    public class CounterDto
    {
        public long Value { get; set; }
        public NotificationType Source { get; set; }
        public int ErrorNumber { get; set; }
        
        public override string ToString()
        {
            return $"{Source}: {Value}";
        }
    }
}