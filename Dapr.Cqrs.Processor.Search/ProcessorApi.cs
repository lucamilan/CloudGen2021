using System;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.Http;
using Dapr.Cqrs.Processor.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Cqrs.Processor.Search {
    public class ProcessorApi {
        private static async Task ExecuteAsync (HttpContext context) {
            Guid? eventId = null;

            try {
                //  Read event for data received from request body
                var payload = await context.Request.ReadFromJsonAsync<SensorDataReceived> (MyDefaults.JsonSerializerOptions);

                //  Get the Id of the Event
                eventId = payload.EventId;

                //throw new ApplicationException("KABOM"); //Is how we can test Drop message

                //  Save data in idempotent manner to our storage
                await context.GetService<AzureBlobManagement> ().CreateIfNotExistsAsync (payload.EventId, payload.Body);
            } catch (RequestFailedException requestFailedException) when (requestFailedException.Status == (int) HttpStatusCode.Conflict) {
                // https://docs.microsoft.com/en-us/rest/api/storageservices/blob-service-error-codes
                Console.WriteLine ($"{requestFailedException.ErrorCode} {requestFailedException.Status}");
            }
            catch (Exception) {
                context.Response.StatusCode = DaprNames.PubSubMessageToBeDropped;
                Console.WriteLine ($"SEARCH issue in processing event {eventId}");
            }

            await context.ConfirmMessageProcessedAsync (eventId, StorageType.Search);
        }
        public static void Map (IEndpointRouteBuilder endpoints) {
            endpoints.MapSubscribeHandler ();
            endpoints.MapPost ("/process-data", ExecuteAsync).WithTopic (DaprNames.PubSub, DaprNames.PubSubTopicSearchStorage);
        }
    }
}