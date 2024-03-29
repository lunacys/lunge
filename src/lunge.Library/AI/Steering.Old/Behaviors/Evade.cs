﻿using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    public class Evade : SteeringComponentBase
    {
        public override Vector2 Steer(ISteeringTarget target)
        {
            var distance = (target.Position - SteeringEntity.Position).Length();
            var updatesAhead = distance / SteeringEntity.MaxVelocity;
            Vector2 futurePos;
            if (target is ISteeringEntity steeringTarget)
                futurePos = target.Position + steeringTarget.Velocity * updatesAhead;
            else 
                futurePos = target.Position;

            return NestedBehavior == null
                ? BehaviorMath.Flee((Vector2SteeringTarget) futurePos, SteeringEntity)
                : NestedBehavior.Steer((Vector2SteeringTarget) futurePos);
        }
    }
}