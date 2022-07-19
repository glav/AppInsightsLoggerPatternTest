using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace appinsightsloggerpatterntest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console with App Settings!");

            var appConfig = LoadAppSettings();

            if (appConfig == null)
            {
                Console.WriteLine("Missing or invalid appsettings.json...exiting");
                return;
            }

            var configValue = appConfig["configValue"];
			Console.WriteLine("Got config value: [{0}]",configValue);

        }
        static IConfigurationRoot LoadAppSettings()
        {
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appConfig = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return appConfig;
        }
    }

}
