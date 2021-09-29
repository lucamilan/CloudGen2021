using System.Text.Json;

namespace Dapr.Cqrs.Common
{
    public static class MyDefaults
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }
}