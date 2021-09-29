using Dapr.Cqrs.Api.Read.Mappers;
using Dapr.Cqrs.Api.Read.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dapr.Cqrs.Api.Read.Controllers
{
    [ApiController]
    [Route("time-data")]
    public class TimeDataController : ControllerBase
    {
        [HttpGet("realtime-aggregates")]
        public async Task<IActionResult> GetRealtimeAggregatesAsync([FromServices] AzureTablesManagement tablesManagement)
        {
            var data = await tablesManagement.GetRealtimeAggregatesAsync();
            await Task.Delay(1);
            //var data = RealtimeAggregatesMapper.Build();

            return new JsonResult(data);
        }

        [HttpGet("top-records/{plantKey}/{numberOfSeconds}")]
        public async Task<IActionResult> GetLastRecordsAsync([FromServices] AzureTablesManagement tablesManagement, string plantKey, int numberOfSeconds = 60)
        {
            var data = await tablesManagement.GetLastRecordsAsync(numberOfSeconds, plantKey);

            return new JsonResult(data);
        }
    }
}