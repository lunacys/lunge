using lunge.Library.Debugging.Logging;
using Nez;
using Playground.Components;
using Playground.Components.Debugging;

namespace Playground.Scenes;

public class TestsScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    private readonly ILogger _logger = LoggerFactory.GetLogger<MainMenuScene>();

    public TestsScene()
    {
        var renderer = AddRenderer(new ScreenSpaceRenderer(100,
            ScreenSpaceRenderLayer));

        AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
    }

    public override void Initialize()
    {
        base.Initialize();

        SetDesignResolution(Width, Height, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(2560, 1440);

        CreateEntity("profiler").AddComponent(new ProfilerComponent()).AddComponent(new LoggerComponent());

        var texture = Content.LoadTexture(Nez.Content.Steering.GrayCircle);

        var cr = 
            CreateEntity("catmull")
                .AddComponent(new CameraMouseHandlerComponent(null))
                .AddComponent(new CatmullRomTests())
                .AddComponent(new LineRenderer())
                .SetEndCapType(EndCapType.Smooth)
                .SetTexture(texture)
                ;
    }
}