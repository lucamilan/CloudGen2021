using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Common.Notification;
using Refit;

namespace Dapr.Cqrs.UI
{
    public interface IAppReadClient
    {
        [Get("/raw-data/{id}")]
        Task<ApiResponse<RawDataDto>> GetRawDataAsync(Guid id);

        [Get("/counters/notification/{notificationType}")]
        Task<ApiResponse<CounterDto>> GetCounterAsync(NotificationType notificationType);

        [Get("/time-data/realtime-aggregates")]
        Task<ApiResponse<IEnumerable<AggregateDto>>> GetRealtimeAggregatesAsync();

        [Get("/time-data/top-records/{plantKey}/{numberOfSeconds}")]
        Task<ApiResponse<IEnumerable<SensorDto>>> GetLastRecordsAsync(int numberOfSeconds, string plantKey);

        [Get("/search-data/{plantKey}/{locationKey}/{tagKey}")]
        Task<ApiResponse<IEnumerable<String>>> GetDirectoriesOrFilesAsync(string plantKey, string locationKey, string tagKey, String prefix);
    }
}