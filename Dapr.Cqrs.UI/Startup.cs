using System.Net.Http;
using Dapr.Cqrs.Core.Hubs;
using Dapr.Cqrs.Core.Uris;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Dapr.Client;

namespace Dapr.Cqrs.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHubConnectionFactory();
            services.AddSingleton(sp =>
            {
                var uris = sp.GetRequiredService<ServiceUrisRegistry>();
                var httpClient = DaprClient.CreateInvokeHttpClient("api-read"); // new HttpClient {BaseAddress = uris.GetReadApi()};
                return RestService.For<IAppReadClient>(httpClient);
            });
            services.AddSingleton(sp =>
            {
                var uris = sp.GetRequiredService<ServiceUrisRegistry>();
                var httpClient = DaprClient.CreateInvokeHttpClient("api-write"); // new HttpClient {BaseAddress = uris.GetWriteApi()};
                return RestService.For<IAppWriteClient>(httpClient);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}