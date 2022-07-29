using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

public class AppInsightsProxy
{
    private readonly string _connectionString;
    private bool _isInitialised = false;
    private static object _lockObject = new object();
    private TelemetryClient _client;

    public bool AutoFlushEnabled {get; set;}

    public AppInsightsProxy(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Trace(string message)
    {
        EnsureInsightsInitialised();


        _client.TrackTrace(message);

        AutoFlushIfEnabled();
    }

    public DependencyTracker TrackDependency(string dependencyTypeName, string dependencyName, string target)
    {
        var tracker = new DependencyTracker(this,dependencyTypeName, dependencyName, target);
        return tracker;

    }

    public void TrackDependency(string dependencyTypeName, string dependencyName, string target, string data, DateTimeOffset startTime, TimeSpan duration, string resultCode, bool success)
    {
        EnsureInsightsInitialised();

        var metadata = new DependencyTelemetry(dependencyTypeName, target, dependencyName, data, startTime, duration, resultCode, success);
        _client.TrackDependency(metadata);

        AutoFlushIfEnabled();
    }

    public void TrackEvent(string eventName, IDictionary<string, string> props = null, IDictionary<string, double> metrics = null)
    {
        EnsureInsightsInitialised();

        _client.TrackEvent(eventName,props,metrics);

        AutoFlushIfEnabled();
    }

    private void AutoFlushIfEnabled()
    {
        if (AutoFlushEnabled)
        {
            _client.Flush();
        }
    }

    public void FlushAll()
    {
        EnsureInsightsInitialised();

        _client.Flush();
    }



    private void EnsureInsightsInitialised()
    {
        if (_isInitialised) return;

        lock (_lockObject)
        {
            if (_isInitialised) return;
            var configuration = TelemetryConfiguration.CreateDefault();
            configuration.ConnectionString = _connectionString;
            _client = new TelemetryClient(configuration);
            _isInitialised = true;

        }

    }
}

