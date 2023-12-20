using lunge.Library.Debugging;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering;

public class SteeringHost : Component
{
    public Vector2 DesiredVelocity;
    public Vector2 Velocity;
    public Vector2 Steering;

    public float MaxVelocity;
    public float MaxForce;
    public float Mass;
    public float Friction;

    public SteeringManager? SteeringManager { get; private set; }

    public SteeringHost()
    {
        MaxVelocity = 180.0f;
        MaxForce = 150f;
        Mass = 1.0f;
        Friction = 0.8f;
        
        Reset();
    }

    public void Reset()
    {
        Velocity = Vector2.Zero;
        Steering = Vector2.Zero;
        DesiredVelocity = Vector2.Zero;
    }

    public override void OnAddedToEntity()
    {
        SteeringManager = Entity.AddComponent(new SteeringManager(this));
    }

    public override void DebugRender(Batcher batcher)
    {
        // Steering Line (Blue)
        batcher.DrawArrow(Entity.Position, Entity.Position + Steering, Color.Blue);
        // Velocity Line (Green)
        batcher.DrawArrow(Entity.Position, Entity.Position + Velocity, Color.Green);
        // Desired Velocity Line (Red)
        batcher.DrawArrow(Entity.Position, Entity.Position + DesiredVelocity * 30, Color.Red);
    }
}