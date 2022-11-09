using System;
using System.Collections;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;
using Nez;
using Playground.Components;
using Playground.Components.Debugging;
using Playground.Components.WorldGeneration;

namespace Playground.Scenes;

public class WorldGenerationScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 2560;
    public static readonly int Height = 1360;

    private readonly ILogger _logger = LoggerFactory.GetLogger<WorldGenerationScene>();

    public WorldGenerationScene()
    {
        AddRenderer(new ScreenSpaceRenderer(100,
            ScreenSpaceRenderLayer));

        AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
    }

    public override void Initialize()
    {
        base.Initialize();

        ClearColor = Color.Black;

        SetDesignResolution(1920, 1080, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(2560, 1340);

        CreateEntity("debugging")
            .AddComponent(new ProfilerComponent())
            .AddComponent(new LoggerComponent())
            .AddComponent(new CameraMouseHandlerComponent());

        CreateEntity("world-generator")
            .AddComponent(new WorldGeneratorComponent(4992, 4992))
            .AddComponent(new WorldRendererComponent());

        Core.StartCoroutine(TestCoroutine1());
        Core.StartCoroutine(TestCoroutine2());
    }

    private int _counter = 0;
    private int _counter2 = 0;

    private IEnumerator TestCoroutine1()
    {
        yield return null;
        while (_counter < 10)
        {
            _counter++;
            Console.WriteLine(" > Coroutine1 counter: " + _counter);
            yield return null;
        }

        Console.WriteLine(" > Coroutine1 Done!");
    }

    private IEnumerator TestCoroutine2()
    {
        yield return null;
        while (_counter2 < 5)
        {
            _counter2++;
            Console.WriteLine(" > Coroutine2 counter: " + _counter2);
            yield return Coroutine.WaitForSeconds(1);
        }

        Console.WriteLine(" > Coroutine2 Done!");
    }
}