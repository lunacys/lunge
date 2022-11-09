using System;
using System.Linq;
using lunge.Library.Utils.Collections;

namespace lunge.Library.Debugging.Profiling;

public class TimeData
{
    public string? Context { get; }
    public TimeSpan LastTime { get; private set; }

    public int MaxSampleSize { get; }
    public int RecalcEvery { get; }

    private readonly CircularArray<TimeSpan> _timeList;

    public double Mean { get; private set; }
    public double Min { get; private set; } = double.MaxValue;
    public double Max { get; private set; }

    public TimeSpan this[int index] => _timeList[index];

    public int CurrentIndex => _timeList._currIndex;

    public int Count => _timeList.Count;

    private int _total = 0;

    public TimeData(TimeSpan last, string? context = null, int maxSampleSize = 64, int recalcEvery = 64)
    {
        _timeList = new CircularArray<TimeSpan>(maxSampleSize);
        LastTime = last;
        Context = context;

        Update(last);
        MaxSampleSize = maxSampleSize;

        if (recalcEvery <= 0)
            recalcEvery = 1;

        RecalcEvery = recalcEvery;
    }

    public void Update(TimeSpan ts)
    {
        LastTime = ts;
        _timeList.Add(ts);

        var tms = ts.TotalMilliseconds;

        if (tms > Max)
            Max = tms;
        else if (tms < Min)
            Min = tms;

        _total++;

        if (_total >= RecalcEvery)
        {
            Recalculate();
            _total = 0;
        }
    }

    public void Clear()
    {
        _timeList.Clear();
    }

    private void Recalculate()
    {
        var tms = _timeList.Select(s => s.TotalMilliseconds).ToArray();

        Mean = tms.Average();
    }

    public override string ToString()
    {
        var res = string.IsNullOrEmpty(Context) ? "" : $"{Context} | ";

        return res + $"Last: {LastTime.TotalMilliseconds:F6} | " +
               $"Mean: {Mean:F6} ({_timeList.Count}) " +
               $"Min: {Min:F6} | Max: {Max:F6}";
    }
}