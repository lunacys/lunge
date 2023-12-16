using System;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Behaviors;

public abstract class BehaviorBase : Component
{
    public SteeringHost Host { get; }

    public bool IsAdditive = true;
    
    public BehaviorBase(SteeringHost host)
    {
        Host = host;
    }

    public abstract Vector2 Steer(SteeringHost target);

    public virtual Vector2 Steer(Vector2 target)
    {
        throw new NotImplementedException();
    }
}