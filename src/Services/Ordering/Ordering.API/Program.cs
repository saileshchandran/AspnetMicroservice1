using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.API.Extensions;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build(); // <--- This is the line that was added
                    host.MigrateDatabase<OrderContext>( (context, services) =>
                    {
                        var logger = services.GetService<ILogger<OrderContextSeed>>();
                        OrderContextSeed
                            .SeedAsync(context, logger)
                            .Wait();
                    }
                );
            host.Run(); // <--- This is the line that was added

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
