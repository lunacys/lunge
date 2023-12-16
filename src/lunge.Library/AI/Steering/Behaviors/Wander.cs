using lunge.Library.AI.Steering.Behaviors.Common;
using Microsoft.Xna.Framework;
using Random = Nez.Random;

namespace lunge.Library.AI.Steering.Behaviors;

public class Wander : BehaviorBase
{
    public float CircleRadius;
    public float CircleDistance;
    public float AngleChange;
    public float WanderAngle;
    
    public Wander(SteeringHost host) : base(host)
    {
    }

    public override void OnAddedToEntity()
    {
        Host.Velocity = Random.RNG.NextVector2(-1, 1);
    }

    public override Vector2 Steer(SteeringHost target)
    {
        return CommonBehaviors.Wander(Host, CircleDistance, CircleRadius, AngleChange, ref WanderAngle);
    }
}