using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors
{
    public class Seek : SteeringComponentBase
    {
        public override Vector2 Steer(ISteeringTarget target)
        {
            return BehaviorMath.Seek(target, SteeringEntity);
        }
    }
}