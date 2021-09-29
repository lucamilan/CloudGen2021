using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Dapr.Cqrs.Core.Uris
{
    public static class ServiceUrisRegistryExtensions
    {
        public static IServiceCollection AddServiceUrisRegistry(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.TryAddSingleton<ServiceUrisRegistry>();
            return serviceCollection;
        }
    }
}