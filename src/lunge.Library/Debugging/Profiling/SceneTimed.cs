using System;
using Nez;

namespace lunge.Library.Debugging.Profiling;

public abstract class SceneTimed : Scene
{
    public string Context { get; }

    protected SceneTimed(string context)
    {
        if (string.IsNullOrEmpty(context))
            throw new ArgumentNullException(nameof(context),
                "Context cannot be null. If you don't want to store time data of Update() method, consider using Nez's Scene base class");

        Context = context;
        GlobalTimeManager.TimeData.Clear();
    }

    public override void Update()
    {
        GlobalTimeManager.TimeAction(() => base.Update(), Context);
    }
}

public abstract class SceneTimed<T> : Scene where T : Scene
{
    public string Context { get; }

    protected SceneTimed()
    {
        Context = typeof(T).Name;
        GlobalTimeManager.TimeData.Clear();
    }

    public override void Update()
    {
        GlobalTimeManager.TimeAction(() => base.Update(), Context);
    }
}