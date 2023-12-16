using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Evade : BehaviorBase
{
    public bool PredictPosition = true;
    
    public Evade(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Evade(Host, target, PredictPosition);
    }
}