using System;
using System.Diagnostics;

public class DependencyTracker : IDisposable
{
    private readonly string _dependencyTypeName;
    private readonly string _dependencyName;
    private readonly string  _target;
    private Stopwatch _timer = new Stopwatch();
    private readonly AppInsightsProxy _proxy;
    private  string _data;
    private  string _resultCode;
    private DateTimeOffset _startTime;
    private bool _isSuccess = false;

    public DependencyTracker(AppInsightsProxy proxy, string dependencyTypeName, string dependencyName, string target)
    {
        _proxy = proxy;
        _dependencyTypeName = dependencyTypeName;
        _dependencyName = dependencyName;
        _target = target;
        _timer.Reset();
        _timer.Start();
        _startTime = DateTimeOffset.Now;
    }

    public void Success(string data = null, string resultCode = null)
    {
        SetResult(data,resultCode,true);
    }
    public void Failure(string data = null, string resultCode = null)
    {
        SetResult(data,resultCode,false);
    }
    private void SetResult(string data, string resultCode, bool isSuccess)
    {
        _data = data;
        _resultCode = resultCode;
        _isSuccess = isSuccess;

    }
    public void Dispose()
    {
        _timer.Stop();
        _proxy.TrackDependency(_dependencyTypeName, _dependencyName, _target, _data,_startTime,_timer.Elapsed,_resultCode,_isSuccess );
    }
}