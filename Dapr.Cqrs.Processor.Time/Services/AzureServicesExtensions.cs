using System;
using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dapr.Cqrs.Processor.Time.Services
{
    public static class AzureServicesExtensions
    {
        public static IServiceCollection AddTimeAzureServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.TryAddSingleton<ConnectionStringsRegistry>();
            serviceCollection.AddSingleton<TimeTablesManagement>();
            return serviceCollection;
        }
    }
}