using System;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Notification;
using Dapr.Cqrs.Core.Counters;
using Dapr.Cqrs.Core.Http;
using Dapr.Cqrs.Core.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dapr.Cqrs.TransactionalOutbox
{
    public static class TransactionalOutboxApi {
        private static async Task RemoveBadMessagesDelegate (HttpContext context) {

            var rows = context.GetService<TransactionalOutboxService> ().RemoveBadMessages ();

            if (rows <= 0) return;

            context.GetService<RedisCountersService> ().Incr (NotificationType.DataRemovedOutbox.ToString (), rows);

            await SafeNotifyAsync (context, NotificationType.DataRemovedOutbox, rows);

            Console.WriteLine ($"Removed {rows} unprocessable messages");
        }

        private static async Task ResendMessagesDelegate (HttpContext context) {
            var rows = context.GetService<TransactionalOutboxService> ().ReSendMessages ();

            if (rows <= 0) return;

            context.GetService<RedisCountersService> ().Incr (NotificationType.DataRetriedOutbox.ToString (), rows);

            await context.GetService<HubConnection> ().NotifyAsync (NotificationType.DataRetriedOutbox, rows);

            await SafeNotifyAsync (context, NotificationType.DataRetriedOutbox, rows);

            Console.WriteLine ($"Sent {rows} messages again");
        }

        private static async Task CallbackMessageDelegate (HttpContext context) {
            Guid? eventId = default;
            try {

                var @event = await context.Request.ReadFromJsonAsync<SensorDataProcessed> (MyDefaults.JsonSerializerOptions);

                eventId = @event.EventId;

                if (!context.GetService<TransactionalOutboxService> ().MarkMessageAsProcessed (@event)) {
                    return;
                }

                context.GetService<RedisCountersService> ().Incr (@event.ConvertToNotificationType ().ToString ());

                await SafeNotifyAsync (context, @event.ConvertToNotificationType (), @event.EventId);

                //Console.WriteLine (@event + " was successfully processed");
            } catch (Exception) {
                context.Response.StatusCode = DaprNames.PubSubMessageToBeRetried3Times;
                Console.WriteLine (eventId + " cannot be processed");
            }
        }
        private static async Task SafeNotifyAsync (HttpContext context, NotificationType notificationType, object message) {
            try {
                await context.GetService<HubConnection> ().NotifyAsync (notificationType, message);
            } catch (Exception exception) {
                Console.WriteLine ($"SafeNotifyAsync failure: {exception.Message}");
            }
        }
        private static Task SendMessagesDelegate (HttpContext context) {
            var messages = context.GetService<TransactionalOutboxService> ().GetMessages ().ToArray ();

            var task = Task.WhenAll(messages.Select(m=>context.GetService<DaprClient> ().PublishEventAsync (DaprNames.PubSub, DaprNames.PubSubTopicName (m.Storage), new SensorDataReceived { EventId = m.Id, Body = m.Data })));

            Console.WriteLine ($"********[{DateTime.Now}]******** Published {messages.Length} messages.");

            return task;
        }
        public static void Map (IEndpointRouteBuilder endpoints) {
            endpoints.MapSubscribeHandler ();
            endpoints.MapPost ("/outbox-send", SendMessagesDelegate);
            endpoints.MapPost ("/outbox-retry", ResendMessagesDelegate);
            endpoints.MapPost ("/outbox-cleanup", RemoveBadMessagesDelegate);
            endpoints.MapPost ("/outbox-callback", CallbackMessageDelegate).WithTopic (DaprNames.PubSub, DaprNames.PubSubTopicOutboxCallback);
        }
    }
}