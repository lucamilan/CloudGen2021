using System;
using System.Threading.Tasks;
using Dapr.Cqrs.Common.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Dapr.Cqrs.Hubs
{
    public class NotificationHub : Hub<INotificationHub>, INotificationHub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public async Task NotifyAsync(NotificationData notification)
        {
            _logger.LogInformation($"Client {Context.ConnectionId} receive {notification}");
            await Clients.All.NotifyAsync(notification);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client Connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"Client Disconnected: {Context.ConnectionId} Exception: {exception?.Message}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}