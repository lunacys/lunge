using lunge.Library.Debugging.Logging;
using Nez;
using Nez.Textures;
using Playground.Components;
using Playground.Components.BitmaskTests;
using Playground.Components.Debugging;

namespace Playground.Scenes;

public class BitmaskingTestsScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    private readonly ILogger _logger = LoggerFactory.GetLogger<BitmaskingTestsScene>();

    public BitmaskingTestsScene()
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

        CreateEntity("profiler")
            .AddComponent(new ProfilerComponent())
            .AddComponent(new LoggerComponent())
            .AddComponent(new CameraMouseHandlerComponent());

        var tilesetTexture = Content.LoadTexture(Nez.Content.Tiles.Testtileset4bit);
        var tileSprites = Sprite.SpritesFromAtlas(tilesetTexture, 32, 32);

        var tileSetDiagTexture = Content.LoadTexture(Nez.Content.Tiles.Testtileset8bit);
        var tileDiagSprites = Sprite.SpritesFromAtlas(tileSetDiagTexture, 32, 32);

        CreateEntity("bitmasking")
            .AddComponent(new BitmaskTestsComponent(tileSprites, tileDiagSprites));

    }
}