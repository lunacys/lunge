using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 Pursuit(SteeringHost host, SteeringHost target, bool predictPos = true)
    {
        var distance = (target.Entity.Position - host.Entity.Position).Length();
        var updatesAhead = distance / host.MaxVelocity;
        Vector2 futurePos;
        if (predictPos)
            futurePos = target.Entity.Position + host.Velocity * updatesAhead;
        else 
            futurePos = target.Entity.Position;

        return Seek(host, futurePos);
    }
    
    public static Vector2 Evade(SteeringHost host, SteeringHost target, bool predictPos = true)
    {
        var distance = (target.Entity.Position - host.Entity.Position).Length();
        var updatesAhead = distance / host.MaxVelocity;
        Vector2 futurePos;
        if (predictPos)
            futurePos = target.Entity.Position + host.Velocity * updatesAhead;
        else 
            futurePos = target.Entity.Position;

        return Flee(host, futurePos);
    }
}