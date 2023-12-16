using System;
using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors;

public class Queue : BehaviorBase
{
    public float MaxQueueRadius;
    public Func<SteeringHost, SteeringHost?> GetNeighborAheadFunc;
    
    public Queue(SteeringHost host, Func<SteeringHost, SteeringHost?> getNeighborAheadFunc) : base(host)
    {
        GetNeighborAheadFunc = getNeighborAheadFunc;
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Queue(Host, GetNeighborAheadFunc, MaxQueueRadius);
    }
}