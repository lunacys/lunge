using System.Collections.Generic;
using lunge.Library.AI.Steering.Behaviors;
using lunge.Library.AI.Steering.Behaviors.Common;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering;

public class SteeringManager : Component, IUpdatable
{
    private SubpixelVector2 _subpixelV2 = new();
    private Mover _mover = null!;

    public SteeringHost Host { get; private set; } = null!;

    private List<BehaviorBase> _behaviors = null!;

    public SteeringManager(SteeringHost host)
    {
        //UpdateOrder = 999;
        Host = host;
    }

    public override void OnAddedToEntity()
    {
        _mover = Entity.AddComponent(new Mover());

        _behaviors = Entity.GetComponents<BehaviorBase>();
    }

    public void Update()
    {
        Host.Steering = MathUtils.Truncate(Host.Steering, Host.MaxForce);
        Host.Steering /= Host.Mass;
        
        Host.Velocity += Host.Steering;
        Host.Velocity = MathUtils.Truncate(Host.Velocity, Host.MaxVelocity);

        DoMove();
        
        //if (Steering == Vector2.Zero)
        Host.Velocity *= Host.Friction;
    }

    private void DoMove()
    {
        var movement = Host.Velocity * Time.DeltaTime;
        
        // TODO: Try AdvancedCalculateMovement
        _mover.CalculateMovement(ref movement, out _);
        _subpixelV2.Update(ref movement);
        _mover.ApplyMovement(movement);
    }

    /// <summary>
    /// Applies (adds) steering force to the entity
    /// </summary>
    /// <example>
    /// SetForce(CommonBehaviors.Flee(new Vector2(16, 32)));
    /// </example>
    /// <param name="force"></param>
    /// <param name="calcDesiredVelocity">Should calculate DesiredVelocity or not</param>
    public void ApplyForce(Vector2 force, bool calcDesiredVelocity = false)
    {
        if (calcDesiredVelocity)
            Host.DesiredVelocity = force * Host.MaxVelocity;
        
        Host.Steering += force;
    }

    /// <summary>
    /// Sets (overrides) steering force of the entity
    /// </summary>
    /// <example>
    /// SetForce(CommonBehaviors.Flee(new Vector2(16, 32)));
    /// </example>
    /// <param name="force"></param>
    /// <param name="calcDesiredVelocity"></param>
    public void SetForce(Vector2 force, bool calcDesiredVelocity = false)
    {
        if (calcDesiredVelocity)
            Host.DesiredVelocity = force * Host.MaxVelocity;

        Host.Steering = force;
    }

    public void RemoveForce()
    {
        Host.Steering = Vector2.Zero;
        Host.DesiredVelocity = Vector2.Zero;
    }

    /// <summary>
    /// Simply sets the velocity vector to the max velocity.
    /// Use this if you don't need complicated movement.
    /// </summary>
    /// <param name="dir"></param>
    public void MoveTo(Vector2 dir)
    {
        Host.Steering = Vector2.Zero;
        Host.DesiredVelocity = dir * Host.MaxVelocity;
        Host.Velocity = Host.DesiredVelocity;
    }

    public void Seek(Vector2 target)
    {
        Host.Steering += CommonBehaviors.Seek(Host, target); 
    }

    public void Seek(SteeringHost target)
    {
        Host.Steering += CommonBehaviors.Seek(Host, target);
    }
}