using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 Seek(SteeringHost host, SteeringHost target)
        => Seek(host, target.Position);

    public static Vector2 Seek(SteeringHost host, Vector2 target)
    {
        var dv = target - host.Position;
        dv.Normalize();

        host.DesiredVelocity = dv;

        return (host.DesiredVelocity * host.MaxVelocity) - host.Velocity;
    }

    public static Vector2 Flee(SteeringHost host, SteeringHost target)
        => Flee(host, target.Position);

    public static Vector2 Flee(SteeringHost host, Vector2 target)
    {
        var dv = (target - host.Position);
        dv.Normalize();
        dv *= host.MaxVelocity;

        host.DesiredVelocity = -dv;

        return host.DesiredVelocity - host.Velocity;
    }

    public static Vector2 Arrival(SteeringHost host, SteeringHost target, float slowingRadius = 20f)
        => Arrival(host, target.Position, slowingRadius);

    public static Vector2 Arrival(SteeringHost host, Vector2 target, float slowingRadius = 20f)
    {
        host.DesiredVelocity = target - host.Position;
        var distance = host.DesiredVelocity.Length();

        host.DesiredVelocity.Normalize();
        
        if (distance < slowingRadius)
            host.DesiredVelocity = host.DesiredVelocity * host.MaxVelocity * (distance / slowingRadius);
        else
            host.DesiredVelocity = host.DesiredVelocity * host.MaxVelocity;

        return host.DesiredVelocity - host.Velocity;
    }
}