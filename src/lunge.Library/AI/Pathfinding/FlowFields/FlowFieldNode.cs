using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Pathfinding.FlowFields
{
    public struct FlowFieldNode
    {
        public int Value { get; set; }
        public Vector2 Velocity { get; set; }

        public FlowFieldNode(int value, Vector2 velocity)
        {
            Value = value;
            Velocity = velocity;
        }
    }
}