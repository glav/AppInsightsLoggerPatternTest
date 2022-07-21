using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class GenericHost : BackgroundService
{
    private ILogger  _logger;
    public GenericHost(ILogger<GenericHost> logger)
    {
        _logger = logger;
    }   
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.InfoMessage("I am a Generic background Host.");

        return Task.CompletedTask;
    }
}
