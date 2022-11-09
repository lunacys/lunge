using Nez;
using Playground.Renderers;

namespace Playground.Scenes;

public class RendererTestsScene : Scene
{
    public RendererTestsScene()
    {
        AddRenderer(new ScreenSpaceRenderer(0, 100));
        AddRenderer(new TestRenderer(-1));
    }

    public override void Initialize()
    {
        base.Initialize();
        
        SetDesignResolution(1440, 900, SceneResolutionPolicy.ShowAllPixelPerfect);
        Screen.SetSize(1440, 900);
        
        
    }
}