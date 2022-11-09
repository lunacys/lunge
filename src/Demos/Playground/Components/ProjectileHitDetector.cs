using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace Playground.Components;

public class ProjectileHitDetector : Component, ITriggerListener
{
    public int HitsUntilDead = 10;

    private int _hitCounter;
    private SpriteRenderer _spriteRenderer;

    public override void OnAddedToEntity()
    {
        _spriteRenderer = Entity.GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter(Collider other, Collider local)
    {
        _hitCounter++;
        if (_hitCounter >= HitsUntilDead)
        {
            Entity.Destroy();
            return;
        }

        _spriteRenderer.Color = Color.Red;
        Core.Schedule(0.1f, timer => _spriteRenderer.Color = Color.White);
    }

    public void OnTriggerExit(Collider other, Collider local)
    {
    }
}