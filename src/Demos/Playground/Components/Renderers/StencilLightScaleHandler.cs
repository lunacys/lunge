using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components.Renderers;

public class StencilLightScaleHandler : Component, IUpdatable
{
    private StencilLight _stencilLight;

    public override void OnAddedToEntity()
    {
        _stencilLight = Entity.GetComponent<StencilLight>();
    }

    public void Update()
    {
        Entity.Scale = new Vector2(Entity.Scene.Camera.RawZoom);
    }
}