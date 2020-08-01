using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Steering
{
    public class PathNode
    {
        public PathNode Next { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public PathNode(Vector2 position, PathNode next, float radius)
        {
            Next = next;
            Position = position;
            Radius = radius;
        }
    }
}