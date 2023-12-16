using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    public static class BehaviorMath
    {
        public static Vector2 Flee(ISteeringTarget target, ISteeringEntity steeringEntity)
        {
            var dv = (target.Position - steeringEntity.Position);
            dv.Normalize();
            dv *= steeringEntity.MaxVelocity;

            steeringEntity.DesiredVelocity = -dv;

            return steeringEntity.DesiredVelocity - steeringEntity.Velocity;
        }

        public static Vector2 Seek(ISteeringTarget target, ISteeringEntity steeringEntity)
        {
            var dv = target.Position - steeringEntity.Position;
            dv.Normalize();

            steeringEntity.DesiredVelocity = dv;

            return (steeringEntity.DesiredVelocity * steeringEntity.MaxVelocity) - steeringEntity.Velocity;
        }
    }
}