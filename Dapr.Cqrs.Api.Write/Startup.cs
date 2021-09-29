using System.Threading.Tasks;
using Dapr.Cqrs.Api.Write.Commands;
using Dapr.Cqrs.Core.Counters;
using Dapr.Cqrs.Core.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dapr.Cqrs.Api.Write
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Dapr.Cqrs Write Api", Version = "v1"});
            });
            services.AddSingleton<WriteDataCommand>()
                .AddRedisCountersService()
                .AddAndStartHubConnection(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr.Cqrs Write Api v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await Task.Delay(1);
                    context.Response.Redirect("/swagger", true);
                });
            });
        }
    }
}