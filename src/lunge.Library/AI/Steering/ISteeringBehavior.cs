using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering
{
    public interface ISteeringBehavior
    {
        ISteeringEntity SteeringEntity { get; set; } 
        Vector2 Steer(ISteeringTarget target);
        Predicate<ConditionArgs> Condition { get; set; }
        ISteeringBehavior NestedBehavior { get; set; }
    }
}