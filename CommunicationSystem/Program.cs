using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using System;

namespace CommunicationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Information("Starting application");
            try
            {
                CreateHostBuilder(args).Build().Run();

                Log.Information("Stopping application");
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An unhandled exception occured during bootstrapping");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Program>();
                    }
                })
            .UseSerilog((context, services, configuration) =>
                {
                    configuration
                            .ReadFrom.Configuration(context.Configuration)
                            .ReadFrom.Services(services)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .WriteTo.Conditional("@l in ['Information']", config =>
                            {
                                config.Logger(x => x.Filter.ByExcluding(Matching.FromSource("Microsoft"))
                                .WriteTo.File(context.Configuration["LoggingPath:Info"]));
                            })
                            .WriteTo.Conditional("@l in ['Warning']", config =>
                            {
                                config.File(context.Configuration["LoggingPath:Warn"]);
                            })
                            .WriteTo.Conditional("@l in ['Error']", config =>
                            {
                                config.File(context.Configuration["LoggingPath:Err"]);
                            });

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                });
    }
}
