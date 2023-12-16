using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class CollisionAvoidance : BehaviorBase
{
    public float MaxAvoidAhead;
    public float AvoidForce;
    public Vector2 Ahead;
    public Vector2 Avoidance;
    
    public CollisionAvoidance(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.CollisionAvoidance(Host, MaxAvoidAhead, AvoidForce, out Ahead, ref Avoidance);
    }
}