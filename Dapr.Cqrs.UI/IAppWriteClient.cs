using System.Threading.Tasks;
using Dapr.Cqrs.Common.Models.Write;
using Refit;

namespace Dapr.Cqrs.UI
{
    public interface IAppWriteClient
    {
        [Post("/")]
        Task PostAsync(SensorData data);
    }
}