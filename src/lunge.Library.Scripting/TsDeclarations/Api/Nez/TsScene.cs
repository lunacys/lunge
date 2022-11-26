using Nez;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsClass(typeof(Scene), "Scene")]
public class TsScene
{
    internal Scene OrigScene { get; set; }

    public Camera Camera;

    public TsScene()
    {
        Camera = OrigScene.Camera;
    }
}