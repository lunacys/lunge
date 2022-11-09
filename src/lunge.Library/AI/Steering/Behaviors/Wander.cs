using System;
using Microsoft.Xna.Framework;
using Nez;
using Random = Nez.Random;

namespace lunge.Library.AI.Steering.Behaviors
{
    public class Wander : SteeringComponentBase
    {
        [Inspectable]
        public float CircleDistance { get; set; }
        
        [Inspectable]
        public float CircleRadius { get; set; }
        
        [Inspectable]
        public float WanderAngle { get; set; }
        
        [Inspectable]
        public float AngleChange { get; set; }
        
        public Wander(float circleDistance, float circleRadius, float wanderAngle, float angleChange)
        {
            CircleDistance = circleDistance;
            CircleRadius = circleRadius;
            WanderAngle = wanderAngle;
            AngleChange = angleChange;
        }

        public override void Initialize()
        {
            base.Initialize();

            SteeringEntity.Velocity = Random.RNG.NextVector2(-1, 1);
            SteeringEntity.DesiredVelocity = Vector2.Zero;
            SteeringEntity.Steering = Vector2.Zero;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            var circleCenter = SteeringEntity.Velocity;
            circleCenter.Normalize();
            circleCenter *= CircleDistance;

            var displacement = new Vector2(0, -1);
            displacement *= CircleRadius;

            displacement = SetAngle(displacement, WanderAngle);

            var nxt = Random.RNG.NextFloat(-AngleChange, AngleChange);
            WanderAngle += nxt;

            var wanderForce = circleCenter + displacement;

            SteeringEntity.DesiredVelocity = wanderForce.Normalized() * SteeringEntity.MaxVelocity;
            return SteeringEntity.DesiredVelocity - SteeringEntity.Velocity;
        }

        private Vector2 SetAngle(Vector2 vec, float value)
        {
            var len = vec.Length();
            vec.X = (float) Math.Cos(value) * len;
            vec.Y = (float) Math.Sin(value) * len;
            return vec;
        }
    }
}