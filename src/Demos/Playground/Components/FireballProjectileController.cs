using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components;

public class FireballProjectileController : Component, IUpdatable
{
    public Vector2 Velocity;
    private ProjectileMover _mover;

    public FireballProjectileController(Vector2 velocity)
    {
        Velocity = velocity;
    }

    public override void OnAddedToEntity()
    {
        _mover = Entity.GetComponent<ProjectileMover>();
    }

    public void Update()
    {
        if (_mover.Move(Velocity * Time.DeltaTime))
            Entity.Destroy();
    }
}