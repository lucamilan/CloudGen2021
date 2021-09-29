using System;
using Dapr.Client;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Core.Dapr;
using Dapr.Cqrs.Processor.Search.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapr.Cqrs.Processor.Search
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSearchAzureServices().AddMyDaprClient();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DaprClient daprClient)
        {
            //app.UseExceptionHandler(c => c.Run(ProcessorApi.AlwaysStatusCodeOkAsync));
            app.UseRouting();
            app.UseCloudEvents();

            app.UseEndpoints(ProcessorApi.Map);

        }
    }
}