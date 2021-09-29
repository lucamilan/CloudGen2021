using Dapr.Cqrs.Core.Dapr;
using Dapr.Cqrs.Processor.Raw.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapr.Cqrs.Processor.Raw
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRawAzureServices().AddMyDaprClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseEndpoints(ProcessorApi.Map);
        }
    }
}