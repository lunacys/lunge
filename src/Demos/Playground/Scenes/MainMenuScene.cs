using lunge.Library.Debugging.Logging;
using lunge.Library.Debugging.Profiling;
using Nez;
using Nez.UI;
using Playground.Components.Debugging;
using Playground.Components.MainMenu;

namespace Playground.Scenes;

public class MainMenuScene : SceneTimed<MainMenuScene>
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    private UICanvas _canvas;

    private readonly ILogger _logger = LoggerFactory.GetLogger<MainMenuScene>();
    
    public MainMenuScene()
    {
        var renderer = AddRenderer(new ScreenSpaceRenderer(100,
            ScreenSpaceRenderLayer));

        AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
    }

    public override void Initialize()
    {
        base.Initialize();

        _canvas = CreateEntity("ui").AddComponent(new MainMenuUiComponent()).AddComponent(new UICanvas());
        //_canvas.IsFullScreen = true;
        _canvas.RenderLayer = ScreenSpaceRenderLayer;

        // ClearColor = Color.LightGray;

        //Core.Instance.Window.IsBorderless = true;

        var ninePatchTexture = Content.LoadTexture("Content/steering/TestNinePatch.png", true);
        var drawable = new NinePatchDrawable(ninePatchTexture, 16, 16, 0, 0);

        var buttonStyle = new TextButtonStyle(drawable, drawable, drawable);

        var skin = Core.Services.GetService<Skin>();
        skin.Add("default", buttonStyle);

        

        SetDesignResolution(Width, Height, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(2560, 1440);

        CreateEntity("profiler").AddComponent(new ProfilerComponent()).AddComponent(new LoggerComponent());

        /*CreateEntity("mouse-entity").AddComponent(new MouseEntityComponent());
        var path = CreateEntity("path-builder").AddComponent(new PathComponent(new Path()));
        CreateEntity("sb-base").AddComponent(new SteeringBehaviorsComponent(path));*/
    }

    public override void Update()
    {
        base.Update();
    }

}