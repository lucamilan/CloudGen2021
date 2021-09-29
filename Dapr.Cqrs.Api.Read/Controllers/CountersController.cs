using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Common.Notification;
using Dapr.Cqrs.Core.Counters;
using Microsoft.AspNetCore.Mvc;

namespace Dapr.Cqrs.Api.Read.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountersController : ControllerBase
    {
        [HttpGet("notification/{notificationType}")]
        public CounterDto GetByNotificationTypeAsync([FromServices] RedisCountersService countersService, NotificationType notificationType)
        {
            try
            {
                return new CounterDto
                {
                    Value = countersService.Read(notificationType.ToString()),
                    Source = notificationType
                };
            }
            catch (ServiceStack.LicenseException)
            {
                return new CounterDto() { ErrorNumber= 1 };
            }
        }
    }
}