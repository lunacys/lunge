using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Playground2.Components.TestsTiledPathfinding
{
    public class Path : IEnumerable<PathNode>
    {
        private readonly List<PathNode> _nodes;

        public int MaxNodes { get; set; }
        public int NodeCount => _nodes.Count;
        public PathNode this[int i] => _nodes[i];

        public Path(int maxNodes = 128)
        {
            _nodes = new List<PathNode>(128);
            MaxNodes = maxNodes;
        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public void AddNode(Vector2 position)
        {
            if (NodeCount < MaxNodes)
            {
                PathNode pathNode = new PathNode(position);
                if (NodeCount > 0)
                    pathNode.NextNode = _nodes[NodeCount - 1];
                _nodes.Add(pathNode);
            }
            else
            {
                RemoveTargetNode();
                AddNode(position);
            }
        }

        public PathNode GetTargetNode()
        {
            if (NodeCount > 0)
                return _nodes[0];
            return null;
        }

        public void RemoveTargetNode()
        {
            if (NodeCount > 0)
                _nodes.Remove(GetTargetNode());
        }

        public IEnumerator<PathNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}