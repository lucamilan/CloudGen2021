using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Notification;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Dapr.Cqrs.Core.Hubs
{
    public static class HubExtensions
    {
        public static async Task NotifyAsync(this HubConnection hub, NotificationType type, object message)
        {
            try
            {
                await hub.InvokeAsync(HubNames.NotificationMethod, new NotificationData
                {
                    Type = type,
                    Message = message?.ToString() ?? ""
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}