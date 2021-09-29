using System;
using System.Collections.Generic;

namespace Dapr.Cqrs.Common.Models.Read
{
    public class AggregateDto
    {
        public string PlantId { get; set; }
        public string TagId { get; set; }
        public string LocationId { get; set; }
        public string PlantLabel { get; set; }
        public string LocationLabel { get; set; }
        public string TagLabel { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public List<AggregateValuesDto> Values { get; set; } = new();
    }
}