using lunge.Library.Debugging;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering;

public class SteeringHost : Entity
{
    public Vector2 DesiredVelocity;
    public Vector2 Velocity;
    public Vector2 Steering;

    public float MaxVelocity;
    public float MaxForce;
    public float Mass;
    public float Friction;

    public SteeringManager SteeringManager { get; private set; } = null!;

    public SteeringHost(string name)
    {
        MaxVelocity = 4.0f;
        MaxForce = 3.8f;
        Mass = 1.0f;
        Friction = 0.8f;
        
        Reset();

        Name = name;
    }

    public void Reset()
    {
        Velocity = Vector2.Zero;
        Steering = Vector2.Zero;
        DesiredVelocity = Vector2.Zero;
    }

    public override void OnAddedToScene()
    {
        SteeringManager = AddComponent(new SteeringManager());
    }

    public override void DebugRender(Batcher batcher)
    {
        // Steering Line (Blue)
        batcher.DrawArrow(Position, Position + Steering * 30, Color.Blue);
        // Velocity Line (Green)
        batcher.DrawArrow(Position, Position + Velocity * 20, Color.Green);
        // Desired Velocity Line (Red)
        batcher.DrawArrow(Position, Position + DesiredVelocity * 30, Color.Red);
    }
}