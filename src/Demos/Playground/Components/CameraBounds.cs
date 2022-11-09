using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components;

public class CameraBounds : Component, IUpdatable
{
    public Vector2 Min, Max;

    public CameraBounds()
    {
        SetUpdateOrder(int.MaxValue);
    }

    public CameraBounds(Vector2 min, Vector2 max) : this()
    {
        Min = min;
        Max = max;
    }

    public override void OnAddedToEntity()
    {
        Entity.UpdateInterval = int.MaxValue;
    }

    public void Update()
    {
        var cameraBounds = Entity.Scene.Camera.Bounds;

        if (cameraBounds.Top < Min.Y)
            Entity.Scene.Camera.Position += new Vector2(0, Min.Y - cameraBounds.Top);

        if (cameraBounds.Left < Min.X)
            Entity.Scene.Camera.Position += new Vector2(Min.X - cameraBounds.Left, 0);

        if (cameraBounds.Bottom > Max.Y)
            Entity.Scene.Camera.Position += new Vector2(0, Max.Y - cameraBounds.Bottom);

        if (cameraBounds.Right > Max.X)
            Entity.Scene.Camera.Position += new Vector2(Max.X - cameraBounds.Right, 0);
    }
}