using Dapr.Cqrs.Core.Counters;
using Dapr.Cqrs.Core.Dapr;
using Dapr.Cqrs.Core.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Cqrs.TransactionalOutbox
{
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {
            services.AddSingleton<TransactionalOutboxService> ()
                .AddRedisCountersService ()
                .AddAndStartHubConnection (Configuration)
                .AddMyDaprClient ();
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseRouting ();
            app.UseCloudEvents ();
            app.UseEndpoints (TransactionalOutboxApi.Map);
        }
    }
}