using Dapr.Cqrs.Core.Uris;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;

namespace Dapr.Cqrs.Core.Hubs
{
    public static class HubConnectionExtensions
    {
        public static IServiceCollection AddHubConnectionFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<ServiceUrisRegistry>();
            services.AddSingleton<HubConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddAndStartHubConnection(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            var uris = new ServiceUrisRegistry(configuration);

            services.TryAddSingleton(uris);

            var hubConnectionFactory = new HubConnectionFactory(uris);

            services.TryAddSingleton(hubConnectionFactory);

            var connection = hubConnectionFactory.Create();

            connection.Closed += async error =>
            {
                Console.WriteLine($"SignalR Connection closed: {error?.Message}");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            Task.Run(async () =>
            {
                while (true)
                    try
                    {
                        await connection.StartAsync();
                        break;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        Console.WriteLine("SignalR Error on Connection");
                    }

                Console.WriteLine($"SignalR connected to {hubConnectionFactory.Url}");
            });

            return services.AddSingleton(connection);
        }
    }
}