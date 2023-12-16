using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Pursuit : BehaviorBase
{
    public bool PredictPosition = true;
    
    public Pursuit(SteeringHost host) : base(host)
    {
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Pursuit(Host, target, PredictPosition);
    }
}