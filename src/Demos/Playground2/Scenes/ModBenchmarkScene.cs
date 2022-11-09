using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lunge.Library.Debugging.Logging;
using Nez;
using Playground2.Components;
using Playground2.Components.ModBenchmark;

namespace Playground2.Scenes;

public class ModBenchmarkScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    private List<IModBenchmark> _modBenchmarks;

    private readonly ILogger _logger = LoggerFactory.GetLogger<ModBenchmarkScene>();

    private readonly int _passCount = 30;

    public ModBenchmarkScene()
    {
        AddRenderer(new ScreenSpaceRenderer(100,
            ScreenSpaceRenderLayer));

        AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
    }

    public override void Initialize()
    {
        base.Initialize();

        // ClearColor = Color.LightGray;

        SetDesignResolution(Width, Height, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(Width, Height);

        _logger.Info($"Initializing benchmarks");

        var profilerEntity = new Entity("profiler");
        profilerEntity.AddComponent(new ProfilerComponent());
        AddEntity(profilerEntity);

        _modBenchmarks = new List<IModBenchmark>
        {
            //new BenchmarkClearScriptComponent(),
            //new BenchmarkJintComponent(),
            //new BenchmarkMoonSharpComponent(),
            //new BenchmarkNLuaComponent()
        };

        _logger.Info($"Starting Mod Benchmark. Pass count: {_passCount}");

        var jsCode = File.ReadAllText("./ModBenchmarkCases/javascript.js");
        var luaCode = File.ReadAllText("./ModBenchmarkCases/lua.lua");

        var meanTimes = new Dictionary<string, double>();

        foreach (var modBenchmark in _modBenchmarks)
        {
            var code = modBenchmark.Lang == Language.JavaScript ? jsCode : luaCode;
            var exec = modBenchmark.GetType().Name.Remove(0, "Benchmark".Length);
            exec = exec.Remove(exec.LastIndexOf('C'));

            _logger.Info($"Starting Benchmark for {exec} (Lang: {modBenchmark.Lang})");

            var allPasses = new List<TimeSpan>();

            for (int i = 0; i < _passCount; i++)
            {
                var ts = modBenchmark.BenchmarkScript(code);

                _logger.Info($"Pass #{i + 1}: {ts.TotalMilliseconds} ms");
                if (i >= 5)
                    allPasses.Add(ts);
            }

            var mean = allPasses.Average(ts => ts.TotalMilliseconds);

            _logger.Info($"Benchmark Done. Mean: {mean} ms\n");

            meanTimes[exec] = mean;
        }

        _logger.Info("Done! Results:");
        foreach (var d in meanTimes)
        {
            _logger.Info($"{d.Key,-12} | Mean: {d.Value:F6}");
        }
        
    }
}