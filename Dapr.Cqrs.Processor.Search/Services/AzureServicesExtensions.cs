using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Dapr.Cqrs.Processor.Search.Services
{
    public static class AzureServicesExtensions
    {
        public static IServiceCollection AddSearchAzureServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.TryAddSingleton<ConnectionStringsRegistry>();
            serviceCollection.TryAddSingleton<AzureBlobManagement>();
            return serviceCollection;
        }
    }
}