using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering;

public class FollowingPathNode
{
    public FollowingPathNode? NextNode { get; set; }
    public Vector2 Target { get; set; }
    public float TargetRadius { get; set; }
    public float ArrivalRadius { get; set; }

    public FollowingPathNode(
        Vector2 position,
        FollowingPathNode? next = null,
        float radius = 6f,
        float arrivalRadius = 32f
    )
    {
        NextNode = next;
        Target = position;
        TargetRadius = radius;
        ArrivalRadius = arrivalRadius;
    }
}