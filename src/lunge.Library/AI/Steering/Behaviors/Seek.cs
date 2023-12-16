using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Seek : BehaviorBase
{
    public Seek(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Seek(Host, target);
    }

    public override Vector2 Steer(Vector2 target)
    {
        return CommonBehaviors.Seek(Host, target);
    }
}