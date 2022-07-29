using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class GenericHost : BackgroundService
{
    private ILogger  _logger;
    private static AppInsightsProxy _proxy;
    private IConfigurationRoot _configRoot;

    public GenericHost(ILogger<GenericHost> logger, IConfigurationRoot configRoot)
    {
        _logger = logger;
        _configRoot = configRoot;
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _proxy = new AppInsightsProxy(_configRoot["appInsightsConnectionString"]);
        return base.StartAsync(cancellationToken);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.InfoMessage("I am a Generic background Host.");
        _logger.InfoMessage("I am using the LoggerMessage pattern from here: https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging");

        LogSomeStuff();

        return Task.CompletedTask;
    }

    private void LogSomeStuff()
    {
        _proxy.Trace("Insights trace message");

        var dummmyMetrics = new Random(DateTime.Now.Millisecond);

        _proxy.TrackEvent("DummyEvent",
            new Dictionary<string,string>(){ {"one","1"},{"two","2"} },
            new Dictionary<string,double>(){ {"oneMetric",dummmyMetrics.NextInt64(100)},{"twoMetric",dummmyMetrics.NextInt64(100)} });

            _proxy.TrackDependency("SQL","SomeDB","SomeTarget","{ \"data\",\"1-2-3\"}",DateTimeOffset.Now,TimeSpan.FromSeconds(2),"200",true);

            // Using disposables to track time within a scope
            using (var tracked = _proxy.TrackDependency("API","SomeApiCall","http://somewhere.com/here"))
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                var delay = rnd.Next(0,1500);
                Thread.Sleep(delay);

                tracked.Success($"Slept {delay} milliseconds","200");
            }

        _proxy.FlushAll();

    }
}
