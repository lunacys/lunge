using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors
{
    public class Flee : SteeringComponentBase
    {
        public override Vector2 Steer(ISteeringTarget target)
        {
            return BehaviorMath.Flee(target, SteeringEntity);
        }
    }
}