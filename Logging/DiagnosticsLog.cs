using System;
using Microsoft.Extensions.Logging;

public static class DiagnosticsLogExtensions
{
    private static readonly Action<ILogger, string, Exception> _infoMessage = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(LoggingEventId.ApplicationGeneraInformationEvent, nameof(InfoMessage)),
            "[Info] {message}");
    

    public static void InfoMessage(this ILogger logger, string message)
    {
        _infoMessage(logger,message,null);
    }

}

