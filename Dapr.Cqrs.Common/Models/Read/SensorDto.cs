using System;

namespace Dapr.Cqrs.Common.Models.Read
{
    public class SensorDto
    {
        public Guid EventId { get; set; }
        public string PlantLabel { get; set; }
        public string LocationLabel { get; set; }
        public string TagLabel { get; set; }
        public double Value { get; set; }        
        public DateTime RecordedOn { get; set; }        
    }
}