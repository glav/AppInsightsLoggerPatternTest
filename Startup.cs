using System;
using System.IO;
using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
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
            host = CreateHostViaAppsettings(appConfig);
        }
        else
        {
            host = CreateHostProgrammatically(appConfig);
        }

        host.Run();

    }

    static IConfigurationRoot LoadAppSettings()
    {
        var appConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.local.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        var appConfigWithInsights = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.local.json", true, true)
            .AddEnvironmentVariables()
            .AddApplicationInsightsSettings(appConfig["appInsightsConnectionString"],true)
            .Build();
        return appConfigWithInsights;
    }

    static IHost CreateHostViaAppsettings(IConfigurationRoot configRoot)
    {
        return Host.CreateDefaultBuilder()
               .ConfigureServices((ctx, builder) =>
               {
                   builder.AddHostedService<GenericHost>();
                   builder.AddSingleton<IConfigurationRoot>(configRoot);
               })
               .Build();

    }

    static IHost CreateHostProgrammatically(IConfigurationRoot configRoot)
    {
        return Host.CreateDefaultBuilder()
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
                   builder.AddSingleton<IConfigurationRoot>(configRoot);
               })
               .Build();

    }
}
