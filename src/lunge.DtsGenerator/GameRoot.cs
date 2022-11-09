using Nez;

namespace lunge.DtsGenerator;

public class GameRoot : Core
{
    protected override void Initialize()
    {
        base.Initialize();

        DebugRenderEnabled = true;

        Scene = new GeneratorTests();
    }
}