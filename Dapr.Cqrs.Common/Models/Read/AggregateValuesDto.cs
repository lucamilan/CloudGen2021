namespace Dapr.Cqrs.Common.Models.Read
{
    public class AggregateValuesDto
    {
        public string IntervalId { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
        public double StandardDeviation { get; set; }
        public int Count { get; set; }
    }
}