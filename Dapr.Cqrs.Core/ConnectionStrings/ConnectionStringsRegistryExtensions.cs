using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Dapr.Cqrs.Core.ConnectionStrings
{
    public static class ConnectionStringsRegistryExtensions
    {
        public static IServiceCollection AddConnectionStringsRegistry(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.TryAddSingleton<ConnectionStringsRegistry>();
            return serviceCollection;
        }
    }
}