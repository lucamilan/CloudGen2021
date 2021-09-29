using Dapr.Cqrs.Common;
using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapr.Cqrs.Hubs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ConnectionStringsRegistry = new ConnectionStringsRegistry(configuration);
        }

        public ConnectionStringsRegistry ConnectionStringsRegistry { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR().AddStackExchangeRedis(ConnectionStringsRegistry.GetRedis());

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>($"/{HubNames.NotificationRoute}");

                endpoints.MapGet("/", async context => { await context.Response.WriteAsync(nameof(Hubs)); });
            });
        }
    }
}