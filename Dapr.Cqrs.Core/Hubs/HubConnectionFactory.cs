using Dapr.Cqrs.Common;
using Dapr.Cqrs.Core.Uris;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Cqrs.Core.Hubs
{
    public class HubConnectionFactory
    {
        private readonly ServiceUrisRegistry _urisRegistry;

        public HubConnectionFactory(ServiceUrisRegistry urisRegistry)
        {
            _urisRegistry = urisRegistry;
        }

        public string Url => _urisRegistry.GetSignalRNotificationHub().ToString();

        public HubConnection Create()
        {
            return new HubConnectionBuilder()
                .WithUrl(Url)
                .WithAutomaticReconnect()
                .AddJsonProtocol(cfg => { cfg.PayloadSerializerOptions = MyDefaults.JsonSerializerOptions; })
                .Build();
        }
    }
}