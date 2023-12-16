using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 Queue(
        SteeringHost host, 
        Func<SteeringHost, SteeringHost?> getNeighborAheadFunc, 
        float maxQueueRadius
        )
    {
        var v = host.Velocity;
        var brake = Vector2.Zero;
        var neighbor = getNeighborAheadFunc(host);

        if (neighbor != null)
        {
            brake = -host.Steering * 0.8f;
            v *= -1;
            brake += v;

            if (Vector2.Distance(host.Position, neighbor.Position) <= maxQueueRadius)
                host.Velocity *= 0.3f;
        }

        return brake;
    }
}