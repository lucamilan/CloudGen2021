using Dapr.Cqrs.Common;
using Microsoft.Extensions.Configuration;
using System;

namespace Dapr.Cqrs.Core.Uris
{
    public sealed class ServiceUrisRegistry
    {
        private readonly IConfiguration _configuration;

        public ServiceUrisRegistry(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Uri GetReadApi()
        {
            return _configuration.GetServiceUri("api-read");
        }

        public Uri GetWriteApi()
        {
            return _configuration.GetServiceUri("api-write");
        }

        public Uri GetSignalR()
        {
            return _configuration.GetServiceUri("signalr-hub");
        }

        public Uri GetSignalRNotificationHub()
        {
            return new Uri($"{GetSignalR()}{HubNames.NotificationRoute}");
        }
    }
}