using Dapr.Cqrs.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Cqrs.Core.Dapr
{
    public static class DaprExtensions
    {
        public static IServiceCollection AddMyDaprClient(this IServiceCollection services)
        {
            services.AddSingleton(MyDefaults.JsonSerializerOptions);

            services.AddDaprClient(builder =>
            {
                builder.UseJsonSerializationOptions(MyDefaults.JsonSerializerOptions);
            });

            return services;
        }
    }
}