namespace Dapr.Cqrs.Common.Notification
{
    public enum NotificationType
    {
        DataInserted,
        DataRetriedOutbox,
        DataRemovedOutbox,
        DataRawProcessing,
        DataTimeProcessing,
        DataSearchProcessing
    }
}