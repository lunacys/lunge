using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 CollisionAvoidance(
        SteeringHost host, 
        float maxAvoidAhead, 
        float avoidForce,
        out Vector2 ahead,
        ref Vector2 avoidance,
        int layerMask = -1
    )
    {
        var dv = host.Velocity;
        if (dv != Vector2.Zero)
            dv.Normalize();
        dv *= maxAvoidAhead * host.Velocity.Length() / host.MaxVelocity;

        ahead = host.Entity.Position + dv;

        // BUG: It really likes to get stuck on edges (rectangle colliders), probably need to check field of view, not just a ray.
        var collision = Physics.Linecast(host.Entity.Position, ahead, layerMask);
        var mostThreatening = collision.Collider;

        if (mostThreatening != null && collision.Collider.Entity != host.Entity)
        {
            avoidance = ahead - mostThreatening.AbsolutePosition;
            avoidance.Normalize();
            avoidance *= avoidForce;
        }
        else
        {
            avoidance *= 0;
        }

        return avoidance;
    }
}