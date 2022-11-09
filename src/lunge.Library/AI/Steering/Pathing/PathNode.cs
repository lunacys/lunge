using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Pathing
{
    public class PathNode
    {
        public PathNode? NextNode { get; set; }
        public Vector2 Target { get; set; }
        public float TargetRadius { get; set; }
        public float ArrivalRadius { get; set; }

        public PathNode(Vector2 position)
            : this(position, null, 6f, 32f)
        {
            
        }

        public PathNode(Vector2 position, PathNode? next, float radius, float arrivalRadius)
        {
            NextNode = next;
            Target = position;
            TargetRadius = radius;
            ArrivalRadius = arrivalRadius;
        }

        public void DebugRender(Batcher batcher)
        {
            batcher.DrawCircle(Target, TargetRadius, Color.Green, 2f);
            batcher.DrawCircle(Target, ArrivalRadius, Color.GreenYellow, 2f);
        }
    }
}