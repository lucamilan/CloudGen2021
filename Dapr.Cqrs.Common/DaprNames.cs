using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Common
{
    public class DaprNames
    {
        public const int PubSubMessageToBeRetried3Times = 422;
        public const int PubSubMessageToBeDropped = 404;
        public const int PubSubMessageWorkIsDone = 200;

        public const string PubSub = "message-bus";
        public const string StateStore = "state-store";
        public const string PubSubTopicBase = "storage_";
        public const string PubSubTopicTimeStorage = "storage_time";
        public const string PubSubTopicRawStorage = "storage_raw";
        public const string PubSubTopicSearchStorage = "storage_search";
        public const string PubSubTopicOutboxCallback = "outbox-callback";



        public static string PubSubTopicName(StorageType storageType) => $"{PubSubTopicBase}{storageType.ToString().ToLowerInvariant()}";

    }
}