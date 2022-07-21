using System;
using System.IO;
using System.Text.Json;
using appinsightsloggerpatterntest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

public class Startup
{
    private const bool USE_APPSETTINGS_CONFIG = true;
    public static void Initialise()
    {
        var appConfig = LoadAppSettings();


        IHost host;
        if (USE_APPSETTINGS_CONFIG)
        {
            var appInsightsConnectionString = appConfig["configValue"];
            if (string.IsNullOrWhiteSpace(appInsightsConnectionString))
            {
                Console.WriteLine("No AppInsights connection string present. No AppInsights logging will be performed.");
            }
            else
            {
                Console.WriteLine("AppInsights connection string present. Logging to AppInsights enabled.");
            }

            host = CreateHostViaAppsettings();
        }
        else
        {
            host = CreateHostProgrammatically();
        }


        host.Run();

    }

    static IConfigurationRoot LoadAppSettings()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var appConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.local.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        return appConfig;
    }

    static IHost CreateHostViaAppsettings()
    {
            return = Host.CreateDefaultBuilder()
                   .ConfigureServices((ctx, builder) =>
                   {
                       builder.AddHostedService<GenericHost>();
                   })
                   .Build();

    }

    static IHost CreateHostProgrammatically()
    {
            return = Host.CreateDefaultBuilder()
                   .ConfigureLogging((ctx, builder) =>
                   {
                       builder.SetMinimumLevel(LogLevel.Debug);
                       builder.AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Warning);
                       builder.AddJsonConsole(x =>
                               {
                                   x.IncludeScopes = true;
                                   x.UseUtcTimestamp = true;
                                   x.JsonWriterOptions = new JsonWriterOptions { Indented = true };
                               });
                   })
                   .ConfigureServices((ctx, builder) =>
                   {
                       builder.AddHostedService<GenericHost>();
                   })
                   .Build();

    }
}
