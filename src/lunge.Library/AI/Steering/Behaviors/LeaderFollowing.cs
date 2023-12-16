using System;
using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class LeaderFollowing : BehaviorBase
{
    public float LeaderBehindDistance;
    public Vector2 Ahead;
    public Vector2 Behind;
    public Func<Vector2, Vector2> SteeringFunc;
    public bool EvadeWhenClose = true;
    public float LeaderSightRadius;

    public LeaderFollowing(SteeringHost host, Func<Vector2, Vector2>? steeringFunc) : base(host)
    {
        SteeringFunc = steeringFunc ?? ((v1) => CommonBehaviors.Seek(host, v1));
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.FollowLeader(
            Host,
            target,
            LeaderBehindDistance,
            out Ahead,
            out Behind,
            SteeringFunc,
            EvadeWhenClose,
            LeaderSightRadius
        );
    }
}