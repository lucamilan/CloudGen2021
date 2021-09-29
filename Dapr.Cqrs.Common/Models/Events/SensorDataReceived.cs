using System;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Common.Models.Events
{
    public class SensorDataReceived
    {
        public Guid EventId { get; set; }
        public SensorData Body { get; set; }

        public override string ToString()
        {
            return $"{EventId} {Body}";
        }
    }
}