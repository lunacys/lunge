using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    /// <summary>
    /// Steering behavior for collision avoidance. Casts a ray towards desired velocity and checks if there's an obstacle.
    /// If there is, tries to avoid it by subtracting desired velocity vector and position of the obstacle.
    /// NOTE: This behavior needs to be supported by other steering behavior (e.g. <see cref="Seek"/>).
    /// </summary>
    public class CollisionAvoidance : SteeringComponentBase
    {
        [Inspectable]
        public float MaxAvoidAhead { get; set; }

        [Inspectable]
        public float AvoidForce { get; set; }

        private Vector2 _ahead;
        private Vector2 _avoidance;
        

        public CollisionAvoidance(float maxAvoidAhead, float avoidForce)
        {
            MaxAvoidAhead = maxAvoidAhead;
            AvoidForce = avoidForce;
        }

        public override void Initialize()
        {
            base.Initialize();

            SteeringEntity.Velocity = new Vector2(-1, -2);
            SteeringEntity.DesiredVelocity = Vector2.Zero;
            SteeringEntity.Steering = Vector2.Zero;
            _ahead = Vector2.Zero;
            _avoidance = Vector2.Zero;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            var dv = SteeringEntity.Velocity;
            if (dv != Vector2.Zero)
                dv.Normalize();
            dv *= MaxAvoidAhead * SteeringEntity.Velocity.Length() / SteeringEntity.MaxVelocity;

            _ahead = SteeringEntity.Position + dv;

            // BUG: It really likes to get stuck on edges (rectangle colliders), probably need to check field of view, not just a ray.
            var collision = Physics.Linecast(SteeringEntity.Position, _ahead, 2);
            var mostThreatening = collision.Collider;

            if (mostThreatening != null && collision.Collider.Entity != Entity)
            {
                _avoidance = _ahead - mostThreatening.AbsolutePosition;
                _avoidance.Normalize();
                _avoidance *= AvoidForce;
            }
            else
            {
                _avoidance *= 0;
            }

            return _avoidance;
        }
    }
}