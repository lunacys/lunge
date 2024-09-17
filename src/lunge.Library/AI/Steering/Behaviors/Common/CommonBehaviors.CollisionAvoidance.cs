using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 CollisionAvoidanceLinecast(
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

    public static Vector2 CollisionAvoidance(
        SteeringHost host,
        Collider? collider,
        float maxAvoidAhead,
        float avoidForce,
        out Vector2 ahead,
        ref Vector2 avoidance,
        int layerMask = -1
    )
    {
        var dv = host.Velocity.NormalizedOrZero() * (maxAvoidAhead * host.Velocity.Length() / host.MaxVelocity);

        ahead = host.Entity.Position + dv;
        
        HashSet<Collider>? neighbors;

        if (collider == null)
        {
            var rect = new RectangleF(host.Entity.Position + ahead, Vector2.One);

            neighbors = Physics.BoxcastBroadphase(rect, layerMask);
        }
        else
        {
            neighbors = Physics.BoxcastBroadphaseExcludingSelf(collider, ahead.X, ahead.Y, layerMask);
        }

        var distance = float.MaxValue;
        Collider? closest = null;

        foreach (var neighbor in neighbors)
        {
            var d = (neighbor.Entity.Position - host.Entity.Position).Length();
            if (d < distance)
            {
                distance = d;
                closest = neighbor;
            }
        }

        if (closest != null)
        {
            avoidance = (ahead - closest.Entity.Position).NormalizedOrZero() * avoidForce;
        }
        else
        {
            avoidance *= 0.0f;
        }

        return avoidance;
    }
}