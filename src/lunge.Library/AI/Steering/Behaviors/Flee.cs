using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Flee : BehaviorBase
{
    public Flee(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Flee(Host, target);
    }

    public override Vector2 Steer(Vector2 target)
    {
        return CommonBehaviors.Flee(Host, target);
    }
}