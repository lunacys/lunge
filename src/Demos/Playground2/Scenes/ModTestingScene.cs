using Nez;
using Playground2.Components.ModTesting;

namespace Playground2.Scenes;

public class ModTestingScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    public ModTestingScene()
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

        var engine = new ModHandlerComponent("C:\\Users\\loonacuse\\source\\repos\\Firebat\\TestMod\\dist");
        var e = new Entity("engine");
        e.AddComponent(engine);
        AddEntity(e);
    }
}