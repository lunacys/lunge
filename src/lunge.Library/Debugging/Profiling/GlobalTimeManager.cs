using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lunge.Library.Debugging.Profiling;

public static class GlobalTimeManager
{
    public static Dictionary<string, TimeData> TimeData { get; } = new();

    [Conditional("DEBUG")]
    public static void AddData(string context, TimeSpan lastTime)
    {
        if (!TimeData.ContainsKey(context))
        {
            TimeData[context] = new TimeData(lastTime, context);
        }
        else
        {
            TimeData[context].Update(lastTime);
        }
    }
    
    public static TimeSpan TimeAction(Action action, string? context = null)
    {
        var sw = new Stopwatch();
        sw.Start();

        action();

        sw.Stop();
        
        var elapsed = sw.Elapsed;

        if (!string.IsNullOrEmpty(context))
            AddData(context, elapsed);

        return elapsed;
    }
    
    public static T TimeFunc<T>(Func<T> func, out TimeSpan elapsed, string? context = null)
    {
        var sw = new Stopwatch();
        sw.Start();

        var res = func();
        sw.Stop();

        elapsed = sw.Elapsed;

        if (!string.IsNullOrEmpty(context))
            AddData(context, elapsed);

        return res;
    }

    public static (T Result, TimeSpan Elapsed) TimeFunc<T>(Func<T> func, string? context = null)
    {
        var res = TimeFunc(func, out var elapsed, context);

        return (res, elapsed);
    }
}