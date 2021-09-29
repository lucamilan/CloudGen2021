using Dapr.Client;
using Dapr.Cqrs.Core.Dapr;
using Dapr.Cqrs.Processor.Time.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapr.Cqrs.Processor.Time
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTimeAzureServices().AddMyDaprClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DaprClient daprClient)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCloudEvents();
            app.UseEndpoints(ProcessorApi.Map);
        }
    }
}