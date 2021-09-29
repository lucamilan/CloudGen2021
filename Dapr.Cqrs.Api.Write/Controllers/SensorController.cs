using System.Threading.Tasks;
using Dapr.Cqrs.Api.Write.Commands;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Common.Notification;
using Dapr.Cqrs.Core.Counters;
using Dapr.Cqrs.Core.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dapr.Cqrs.Api.Write.Controllers
{
    [Route("")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly WriteDataCommand _command;
        private readonly RedisCountersService _countersService;
        private readonly HubConnection _hubConnection;

        public SensorController(WriteDataCommand command, RedisCountersService countersService,
            HubConnection hubConnection)
        {
            _command = command;
            _countersService = countersService;
            _hubConnection = hubConnection;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(SensorData sensorData)
        {
            if (!_command.Execute(sensorData)) return NoContent();
            
            const NotificationType notificationType = NotificationType.DataInserted;

            _countersService.Incr(notificationType.ToString());

            await _hubConnection.NotifyAsync(notificationType, sensorData.ReadableData());

            return NoContent();
        }
    }
}