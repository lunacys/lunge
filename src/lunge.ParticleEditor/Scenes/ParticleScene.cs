using lunge.ParticleEditor.Components;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.ParticleEditor.Scenes;

public class ParticleScene : Scene
{
    public override void Initialize()
    {
        ClearColor = Color.Black;
        
        SetDesignResolution(1600, 900, SceneResolutionPolicy.None);
        Screen.SetSize(1600, 900);

        var particlesEntity = CreateEntity("particles-entity");
        particlesEntity.SetPosition(Screen.Center - new Vector2(0, 200));
        particlesEntity.AddComponent(new ParticleSystemSelectorComponent());
        particlesEntity.AddComponent(new SimpleMover());

        // TODO: 
        //  1) Show all available particle presets
        //  2) Handle Sprites
        //  3) Import *.pex files
    }
}