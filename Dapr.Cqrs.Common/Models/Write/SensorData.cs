using System;

namespace Dapr.Cqrs.Common.Models.Write
{
    public class SensorData
    {
        public string Plant { get; set; }
        public string Location { get; set; }
        public string Tag { get; set; }
        public double Value { get; set; }
        public DateTime RecordedOn { get; set; }

        public override string ToString()
        {
            return $"{Plant} {Location} {Tag} {Value} {RecordedOn}";
        }
    }
}