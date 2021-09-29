using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceStack.Redis;

namespace Dapr.Cqrs.Core.Counters
{
    public static class RedisCountersServiceExtensions
    {
        public static IServiceCollection AddRedisCountersService(this IServiceCollection services)
        {
            services.TryAddSingleton<ConnectionStringsRegistry>();
            services.AddTransient<IRedisClientsManager, PooledRedisClientManager>(sp =>
                new PooledRedisClientManager(15, sp.GetRequiredService<ConnectionStringsRegistry>().GetRedis()));
            services.AddSingleton<RedisCountersService>();
            return services;
        }
    }
}