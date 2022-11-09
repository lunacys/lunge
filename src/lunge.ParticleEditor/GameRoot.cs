using lunge.ParticleEditor.Scenes;
using Nez;

namespace lunge.ParticleEditor;

public class GameRoot : Core
{
    protected override void Initialize()
    {
        base.Initialize();

        Scene = new ParticleScene();
    }
}
