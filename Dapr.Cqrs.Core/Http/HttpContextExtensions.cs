using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Models.Write;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Cqrs.Core.Http {
    public static class HttpContextExtensions {

        public static async Task<string> GetRawBodyAsync (this HttpContext context, Encoding encoding = null) {
            var request = context.Request;

            if (!request.Body.CanSeek) {
                request.EnableBuffering ();
            }

            request.Body.Position = 0;

            var reader = new StreamReader (request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync ().ConfigureAwait (false);

            request.Body.Position = 0;

            return body;
        }
        public static T GetService<T> (this HttpContext context) {
            if (context == null) throw new ArgumentNullException (nameof (context));
            return context.RequestServices.GetRequiredService<T> ();
        }

        public static async Task ConfirmMessageProcessedAsync (this HttpContext context, Guid? eventId, StorageType storageType) {
            if (eventId is null) return;

            try {
                //  Create event for data processed 
                var @event = new SensorDataProcessed {
                    EventId = eventId.Value,
                    StorageType = storageType
                };

                await context.GetService<DaprClient> ().PublishEventAsync (DaprNames.PubSub, DaprNames.PubSubTopicOutboxCallback, @event);
            } catch (Exception ex) {
                context.Response.StatusCode = DaprNames.PubSubMessageToBeRetried3Times;
                Console.WriteLine ($"DAPR was not able to send message. Reason: {ex.Message}");
            }
        }
    }
}