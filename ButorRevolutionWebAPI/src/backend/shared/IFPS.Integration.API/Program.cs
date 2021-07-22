using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace IFPS.Integration.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
               .AddEnvironmentVariables()
               .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
               .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Console.WriteLine(msg);
            });

            try
            {
                Log.Information("Host starting...");

                CreateWebHostBuilder(args)
                .UseSerilog()
                .Build()
                .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .AddConfiguration(Configuration)
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .Build();
            })
            .UseStartup<Startup>();
    }
}