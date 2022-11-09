using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components.Renderers;

public class StencilLightMapHandler : Component, IUpdatable
{
    private Scene _scene;
    private Camera _camera;

    public StencilLightMapHandler()
    { }

    public override void OnAddedToEntity()
    {
        _scene = Entity.Scene;
        _camera = _scene.Camera;
    }

    public void Update()
    {
        Entity.Position = _camera.Position;
        Entity.Scale = new Vector2(1 / (_camera.RawZoom));
    }
}