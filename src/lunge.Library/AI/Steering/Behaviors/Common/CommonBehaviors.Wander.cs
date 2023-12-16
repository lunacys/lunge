using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Random = Nez.Random;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    /// <summary>
    /// Wanders around without any target.
    /// Needs a setup like this:
    /// <code>Host.Velocity = Random.RNG.NextVector2(-1, 1);</code>
    /// </summary>
    /// <param name="host"></param>
    /// <param name="circleDistance"></param>
    /// <param name="circleRadius"></param>
    /// <param name="angleChange"></param>
    /// <param name="wanderAngle"></param>
    /// <returns></returns>
    public static Vector2 Wander(
        SteeringHost host, 
        float circleDistance, 
        float circleRadius,
        float angleChange,
        ref float wanderAngle,
        Vector2? displacementV = null
        )
    {
        var circleCenter = host.Velocity;
        circleCenter.Normalize();
        circleCenter *= circleDistance;

        
        var displacement = displacementV.HasValue ? displacementV.Value : new Vector2(0, -1);
        displacement *= circleRadius;

        displacement = MathUtils.SetAngle(displacement, wanderAngle);

        var nxt = Random.RNG.NextFloat(-angleChange, angleChange);
        wanderAngle += nxt;
        wanderAngle = MathHelper.WrapAngle(wanderAngle);

        var wanderForce = circleCenter + displacement;

        host.DesiredVelocity = wanderForce.Normalized() * host.MaxVelocity;
        return host.DesiredVelocity - host.Velocity;
    }
}