using System;
using System.IO;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public static void Initialise()
    {
        var appConfig = LoadAppSettings();

        if (appConfig == null)
        {
            Console.WriteLine("Missing or invalid appsettings.json...exiting");
            return;
        }

        var appInsightsConnectionString = appConfig["configValue"];
        if (string.IsNullOrWhiteSpace(appInsightsConnectionString))
        {
            Console.WriteLine("No AppInsights connection string present. No AppInsights logging will be performed.");
        }
        else
        {
            Console.WriteLine("AppInsights connection string present. Logging to AppInsights enabled.");
        }

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

}