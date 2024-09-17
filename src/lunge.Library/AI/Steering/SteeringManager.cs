using lunge.Library.Utils;
using Nez;

namespace lunge.Library.AI.Steering;

public class SteeringManager : Component, IUpdatable
{
    private SubpixelVector2 _subpixelV2;
    private Mover _mover = null!;

    public SteeringHost Host { get; }

    public SteeringManager(SteeringHost host)
    {
        Host = host;
        UpdateOrder = 99;
    }

    public override void OnAddedToEntity()
    {
        _mover = Entity.AddComponent(new Mover());
    }

    public void Update()
    {
        Steer();
        UpdatePosition();
        ApplyFriction();
    }

    private void Steer()
    {
        Host.Steering = MathUtils.Truncate(Host.Steering, Host.MaxForce);
        Host.Steering /= Host.Mass;

        Host.Velocity = MathUtils.Truncate(Host.Velocity + Host.Steering, Host.MaxVelocity);
    }

    private void UpdatePosition()
    {
        var movement = Host.Velocity * Time.DeltaTime;
        _mover.CalculateMovement(ref movement, out _);
        _subpixelV2.Update(ref movement);
        _mover.ApplyMovement(movement);
    }

    private void ApplyFriction()
    {
        Host.Velocity *= Host.Friction;
    }
}