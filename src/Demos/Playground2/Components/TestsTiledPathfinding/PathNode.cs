using Microsoft.Xna.Framework;

namespace Playground2.Components.TestsTiledPathfinding
{
    public class PathNode
    {
        public PathNode NextNode { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public PathNode(Vector2 position, float radius = 6f)
            : this(position, radius, null)
        { }

        public PathNode(Vector2 position, float radius, PathNode nextNode)
        {
            Position = position;
            Radius = radius;
            NextNode = nextNode;
        }
    }
}