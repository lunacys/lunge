using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using lunge.Library.Debugging.Profiling;
using Nez;
using Nez.ImGuiTools;

namespace Playground2.Components;

public class ProfilerComponent : Component
{
    //private float[] _array = null;

    private readonly Dictionary<string, float[]> _timeCache = new ();

    private bool _isBreak = false;

    public ProfilerComponent()
    {
        
    }

    public override void OnAddedToEntity()
    {
        Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(Draw);
    }

    private void Draw()
    {
        ImGui.Begin("Profiling");

        ImGui.Text($"Delta Time: {Time.DeltaTime:F6}. Time Scale: {Time.TimeScale:F1}. Frame Count: {Time.FrameCount}. Total Time: {Time.TotalTime:F2}. A: {Time.DeltaTime / (1 / 60f):F6}");

        foreach (var timeData in GlobalTimeManager.TimeData)
        {
            if (!_timeCache.ContainsKey(timeData.Key))
                _timeCache[timeData.Key] = new float[timeData.Value.MaxSampleSize];

            if (!_isBreak)
                _timeCache[timeData.Key][timeData.Value.CurrentIndex] = (float)timeData.Value.LastTime.TotalMilliseconds;

            //ImGui.Text($"{timeData.Key}: {timeData.Value.LastTime.TotalMilliseconds:F8}");
            ImGui.Text($"{timeData.Value}");
            ImGui.PlotLines(
                $"##{timeData.Value}",
                ref _timeCache[timeData.Key][0],
                timeData.Value.MaxSampleSize,
                timeData.Value.CurrentIndex,
                "",
                0.001f,
                1f,
                new Vector2(500, 100)
            );
        }

        if (ImGui.Button("Break"))
        {
            Time.TimeScale = 0;
            _isBreak = !_isBreak;
            if (_isBreak)
                Time.TimeScale = 0;
            else
                Time.TimeScale = 1;
        }

        ImGui.End();
    }
}