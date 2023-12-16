using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Arrival : BehaviorBase
{
    public float SlowingRadius = 20f;
    
    public Arrival(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Arrival(Host, target, SlowingRadius);
    }

    public override Vector2 Steer(Vector2 target)
    {
        return CommonBehaviors.Arrival(Host, target, SlowingRadius);
    }
}