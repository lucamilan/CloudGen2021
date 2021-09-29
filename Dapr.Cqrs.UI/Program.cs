using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Dapr.Cqrs.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("it-IT");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}