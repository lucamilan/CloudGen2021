using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;

namespace Dapr.Cqrs.Processor.Time.Models
{
    public class TimeAggregateTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string PlantLabel { get; set; }
        public string LocationLabel { get; set; }
        public string TagLabel { get; set; }

        public double Min0 { get; set; }
        public double Max0 { get; set; }
        public double Average0 { get; set; }
        public double StdDev0 { get; set; }
        public int Count0 { get; set; }


        public double Min1 { get; set; }
        public double Max1 { get; set; }
        public double Average1 { get; set; }
        public double StdDev1 { get; set; }
        public int Count1 { get; set; }

        public double Min2 { get; set; }
        public double Max2 { get; set; }
        public double Average2 { get; set; }
        public double StdDev2 { get; set; }
        public int Count2 { get; set; }

        public double Min3 { get; set; }
        public double Max3 { get; set; }
        public double Average3 { get; set; }
        public double StdDev3 { get; set; }
        public int Count3 { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        private const int Interval0 = 30;
        private const int Interval1 = 60;
        private const int Interval2 = 120;
        private const int Interval3 = 300;
        public static int[] Intervals = { Interval0, Interval1, Interval2, Interval3 };

        private static double CalculateStandardDeviation(double[] sequence)
        {
            if (!sequence.Any())
            {
                return 0;
            }
            double average = sequence.Average();
            double sum = sequence.Sum(d => Math.Pow(d - average, 2));
            //double std = Math.Sqrt((sum) / (sequence.Count() - 1)); //evaluate as a sample.
            double std = Math.Sqrt((sum) / (sequence.Count() )); //evaluate as a population.
            return std;
        }


        public static void Update(int interval, TimeAggregateTableEntity entity, TimeRecordTableEntity[] records)
        {
            var max = records.Length == 0 ? 0 : records.Max(_ => _.Value);
            var min = records.Length == 0 ? 0 : records.Min(_ => _.Value);
            var avg = Math.Round(records.Length == 0 ? 0 : records.Select(_ => _.Value).Average(), 2);
            var std = CalculateStandardDeviation(records.Select(_ => _.Value).ToArray());
            var count = records.Length == 0 ? 0 : records.Length;

            switch (interval)
            {
                case Interval0:
                    entity.Max0 = max;
                    entity.Min0 = min;
                    entity.Average0 = avg;
                    entity.StdDev0 = std;
                    entity.Count0 = count;
                    break;
                case Interval1:
                    entity.Max1 = max;
                    entity.Min1 = min;
                    entity.Average1 = avg;
                    entity.StdDev1 = std;
                    entity.Count1 = count;
                    break;
                case Interval2:
                    entity.Max2 = max;
                    entity.Min2 = min;
                    entity.Average2 = avg;
                    entity.StdDev2 = std;
                    entity.Count2 = count;
                    break;
                case Interval3:
                    entity.Max3 = max;
                    entity.Min3 = min;
                    entity.StdDev3 = std;
                    entity.Average3 = avg;
                    entity.Count3 = count;
                    break;
            }
        }
    }
}