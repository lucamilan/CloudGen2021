using System;

namespace Dapr.Cqrs.Core
{
    public static class MiscExtensions
    {
        public static string BuildQueryFilterForTimeScan(string partitionKey, int intervalInSeconds = 0)
        {
            var partitionLowerBound =partitionKey;
            var partitionUpperBound  = CreateUpperBoundString(partitionKey);
            var rowKey = $"{DateTime.MaxValue.Ticks - DateTime.UtcNow.AddSeconds(-1 * intervalInSeconds).Ticks:D19}";
            var filter = $" (PartitionKey ge '{partitionLowerBound}' and PartitionKey le '{partitionUpperBound}') and '{rowKey}' ge RowKey";
            return filter;
        }

        private static string CreateUpperBoundString(string lowerBoundString)
        {
            return lowerBoundString.Substring(0, lowerBoundString.Length - 1) + (char)(lowerBoundString[^1] + 1);
        }
    }
}