using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Entities.Steering
{
    public class Path
    {
        public List<PathNode> Nodes { get; } = new List<PathNode>();

        public int MaxNodes { get; set; }
        public int NodeCount => Nodes.Count;
        public PathNode this[int i] => Nodes[i];

        public Path(int maxNodes = 128)
        {
            MaxNodes = maxNodes;
        }

        public void AddNode(Vector2 position, float radius)
        {
            if (NodeCount < MaxNodes)
            {
                PathNode pn = new PathNode(position, null, radius);
                if (NodeCount != 0)
                    pn.Next = Nodes[NodeCount - 1];
                Nodes.Add(pn);
            }
            else
            {
                RemoveTargetNode();
                AddNode(position, radius);
            }
        }

        public void Clear()
        {
            Nodes.Clear();
        }

        public PathNode GetTargetNode()
        {
            if (NodeCount > 0)
                return Nodes.First();
            return null;
        }

        public void RemoveTargetNode()
        {
            if (NodeCount > 0)
                Nodes.Remove(GetTargetNode());
        }

        /*public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var n in _nodes)
                n.Draw(spriteBatch);
            DrawLines(spriteBatch);
        }

        private void DrawLines(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _nodes.Count - 1; i++)
                spriteBatch.DrawLine(
                    _nodes[i].Circle.Center,
                    _nodes[i + 1].Circle.Center,
                    Color.Yellow,
                    2);
        }*/
    }
}