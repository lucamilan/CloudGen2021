using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.Http;
using Dapr.Cqrs.Processor.Time.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dapr.Cqrs.Processor.Time
{
    public class ProcessorApi {
        private static async Task ExecuteAsync (HttpContext context) {
            Guid? eventId = default;
            
            try {
                //  Read event data received from request body
                var payload = await context.Request.ReadFromJsonAsync<SensorDataReceived> (MyDefaults.JsonSerializerOptions);

                //  Get the Id of the Event
                eventId = payload.EventId;

                //throw new ApplicationException("KABOM"); //Is how we can test Drop message

                //  Save data in idempotent manner to our storage
                await context.GetService<TimeTablesManagement> ().CreateRecordForTimeAsync (payload.EventId, payload.Body);
            } catch (RequestFailedException requestFailedException) when (requestFailedException.Status == (int) HttpStatusCode.Conflict) {
                //  https://docs.microsoft.com/en-us/rest/api/storageservices/table-service-error-codes
                Console.WriteLine ($"{requestFailedException.ErrorCode} {requestFailedException.Status}");
            }
            catch (Exception ex) {
                eventId = default;
                context.Response.StatusCode = DaprNames.PubSubMessageToBeDropped;
                Console.WriteLine ($"TIME issue in processing event {eventId} {ex}");
            }

            //  confirm tha message has been processed to Outbox service
            await context.ConfirmMessageProcessedAsync (eventId, StorageType.Time);
        }

        private static async Task ExecuteAggregatorAsync (HttpContext context) {
            try {
                var tablesManagement = context.GetService<TimeTablesManagement> ();

                var tableForTimeRecords = await tablesManagement.GetTableClientAsync (AzureTablesNames.TableOptimizedForQueryTimeBased);

                var entities = await tablesManagement.ProcessDataByIntervalsAsync (tableForTimeRecords);

                var tableForRealtimeAggregates = await tablesManagement.GetTableClientAsync (AzureTablesNames.MaterializedViewForRealtimeAggregates);

                await Task.WhenAll(entities.Select(entity=>tableForRealtimeAggregates.UpsertEntityAsync (entity, TableUpdateMode.Replace)));

                // foreach (var entity in entities) {
                //     await tableForRealtimeAggregates>.UpsertEntityAsync (entity, TableUpdateMode.Replace);
                // }

                Console.WriteLine ("Aggregate time data");
            } catch (Exception exception) {
                Console.WriteLine ($"********[OPS]******** Failure {exception.Message}");
            }
        }

        public static void Map (IEndpointRouteBuilder endpoints) {
            endpoints.MapSubscribeHandler ();
            endpoints.MapPost ("/process-data", ExecuteAsync).WithTopic (DaprNames.PubSub, DaprNames.PubSubTopicTimeStorage);
            endpoints.MapPost ("/aggregate-data", ExecuteAggregatorAsync);
        }
    }
}