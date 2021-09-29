using System.ComponentModel;

namespace Dapr.Cqrs.Common.Models.Write
{
    public enum StorageType
    {
        [Description("Azure Blob")] Raw = 1,
        [Description("TimescaleDb")] Time = 2,
        [Description("MongoDb")] Search = 3
    }
}