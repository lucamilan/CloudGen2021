using System;

namespace Dapr.Cqrs.Common.Models.Read
{
    public class RawDataDto
    {
        public Guid Id { get; set; }
        public string Json { get; set; }
        public string Type { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}