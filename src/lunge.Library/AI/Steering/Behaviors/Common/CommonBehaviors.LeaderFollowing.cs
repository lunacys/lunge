using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 FollowLeader(
        SteeringHost host,
        SteeringHost leader, 
        float leaderBehindDist,
        out Vector2 ahead,
        out Vector2 behind,
        Func<Vector2, Vector2> resolveFunc,
        bool evadeWhenClose = false,
        float leaderSightRadius = 32f)
    {
        var dv = leader.Velocity;
        var force = Vector2.Zero;

        dv.Normalize();
        dv *= leaderBehindDist;
        ahead = leader.Entity.Position + dv;

        dv *= -1;
        behind = leader.Entity.Position + dv;

        if (evadeWhenClose && IsOnLeaderSight(host, leader, ahead, leaderSightRadius))
            force += Evade(host, leader);

        return force + resolveFunc(behind);
    }

    private static bool IsOnLeaderSight(SteeringHost host, SteeringHost leader, Vector2 leaderAhead, float leaderSightRadius)
    {
        return Vector2.Distance(leaderAhead, host.Entity.Position) <= leaderSightRadius ||
               Vector2.Distance(leader.Entity.Position, host.Entity.Position) <= leaderSightRadius;
    }
}