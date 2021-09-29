using System;
using System.Net;
using System.Threading.Tasks;
using Azure;
using CloudNative.CloudEvents.AspNetCore;
using CloudNative.CloudEvents.SystemTextJson;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.Http;
using Dapr.Cqrs.Processor.Raw.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace Dapr.Cqrs.Processor.Raw
{
    public class ProcessorApi {
        private static readonly JsonEventFormatter CloudEventJsonFormatter = new JsonEventFormatter();
        public static async Task ExecuteAsync (HttpContext context) {
            Guid? eventId = default;
            try {
                //var rawBody = await context.GetRawBodyAsync();

                //Console.WriteLine(rawBody);

                //  Read cloud event received from request body
                var cloudEvent = await context.Request.ToCloudEventAsync(CloudEventJsonFormatter);

                //  Parse domain event
                var payload = JsonSerializer.Deserialize<SensorDataReceived>(cloudEvent.Data.ToString(), MyDefaults.JsonSerializerOptions);

                //  Get the Id of the Event
                eventId = payload.EventId;

                //throw new ApplicationException("KABOM"); //Is how we can test Drop message

                //  Save data in idempotent manner to our storage
                await context.GetService<AzureBlobManagement> ().CreateIfNotExistsAsync (payload.EventId, payload.Body);

            } catch (RequestFailedException requestFailedException) when (requestFailedException.Status == (int) HttpStatusCode.Conflict) {
                // https://docs.microsoft.com/en-us/rest/api/storageservices/blob-service-error-codes
                Console.WriteLine ($"{requestFailedException.ErrorCode} {requestFailedException.Status}");
            }
            catch (Exception ex) {
                eventId = default;
                context.Response.StatusCode = DaprNames.PubSubMessageToBeDropped;
                Console.WriteLine ($"RAW issue in processing event {eventId} {ex}");
            }

            //  confirm tha message has been processed to Outbox service
            await context.ConfirmMessageProcessedAsync (eventId, StorageType.Raw);
        }
        public static void Map (IEndpointRouteBuilder endpoints) {
            endpoints.MapSubscribeHandler ();
            endpoints.MapPost ("/process-data", ExecuteAsync).WithTopic (DaprNames.PubSub, DaprNames.PubSubTopicRawStorage);
        }
    }
}