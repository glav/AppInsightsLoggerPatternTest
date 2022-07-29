using System;
using System.Diagnostics;

public class ActivityTracker : IDisposable
{
    private static ActivitySource source = new ActivitySource(typeof(ActivityTracker).AssemblyQualifiedName);

    private readonly string _dependencyTypeName;
    private readonly string _dependencyName;
    private readonly string  _target;
    private Stopwatch _timer = new Stopwatch();
    private readonly AppInsightsProxy _proxy;
    private  string _data;
    private  string _resultCode;
    private DateTimeOffset _startTime;
    private bool _isSuccess = false;

    public ActivityTracker(AppInsightsProxy proxy, string dependencyTypeName, string dependencyName, string target)
    {
        _proxy = proxy;
        _dependencyTypeName = dependencyTypeName;
        _dependencyName = dependencyName;
        _target = target;
        _timer.Reset();
        _timer.Start();
        _startTime = DateTimeOffset.Now;

        
    }

    public void Dispose()
    {
        _timer.Stop();
        _proxy.TrackDependency(_dependencyTypeName, _dependencyName, _target, _data,_startTime,_timer.Elapsed,_resultCode,_isSuccess );
    }
}