using System.Threading.Tasks;

namespace Dapr.Cqrs.Common.Notification
{
    public interface INotificationHub
    {
        Task NotifyAsync(NotificationData notification);
    }
}