using Nez;

namespace Playground.Components;

public class CameraMouseHandlerComponent : Component, IUpdatable
{
    public Camera? Camera { get; private set; }

    public CameraMouseHandlerComponent(Camera? camera = null)
    {
        Camera = camera;
    }

    public override void OnAddedToEntity()
    {
        if (Camera == null)
            Camera = Entity.Scene.Camera;
    }

    public void Update()
    {
        if (Input.MiddleMouseButtonDown)
        {
            Camera!.Position -= Input.MousePositionDelta.ToVector2() / Camera!.RawZoom;
        }

        if (Input.MouseWheelDelta > 0)
        {
            Camera!.ZoomIn(0.25f);
        }
        else if (Input.MouseWheelDelta < 0)
        {
            Camera!.ZoomOut(0.25f);
        }
    }
}