using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Behaviors
{
    public class Arrival : SteeringComponentBase
    {
        [Inspectable]
        public float SlowingRadius { get; set; }

        private ISteeringTarget _target;
        
        public Arrival(float slowingRadius)
        {
            SlowingRadius = slowingRadius;
        }
        
        public override Vector2 Steer(ISteeringTarget target)
        {
            if (_target == null)
                _target = target;
            
            SteeringEntity.DesiredVelocity = target.Position - SteeringEntity.Position;
            var distance = SteeringEntity.DesiredVelocity.Length();

            if (distance < SlowingRadius)
                SteeringEntity.DesiredVelocity = SteeringEntity.DesiredVelocity.Normalized() * SteeringEntity.MaxVelocity * (distance / SlowingRadius);
            else
                SteeringEntity.DesiredVelocity = SteeringEntity.DesiredVelocity.Normalized() * SteeringEntity.MaxVelocity;

            return SteeringEntity.DesiredVelocity - SteeringEntity.Velocity;
        }

        public override void DebugRender(Batcher batcher)
        {
            if (_target != null)
                batcher.DrawCircle(_target.Position, SlowingRadius, Color.Red);
            
            base.DebugRender(batcher);
        }
    }
}