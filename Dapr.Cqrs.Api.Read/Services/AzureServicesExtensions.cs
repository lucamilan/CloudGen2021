using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Dapr.Cqrs.Api.Read.Services

{
    public static class AzureServicesExtensions
    {
        public static IServiceCollection AddReadAzureServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.TryAddSingleton<ConnectionStringsRegistry>();
            serviceCollection.AddSingleton<AzureBlobManagement>();
            serviceCollection.AddSingleton<AzureTablesManagement>();
            return serviceCollection;
        }
    }
}