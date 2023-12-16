using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering;

public class FollowingPath : IEnumerable<FollowingPathNode>
{
    private readonly List<FollowingPathNode> _nodes;
    
    public int MaxNodes { get; set; }
    public int NodeCount => _nodes.Count;
    public FollowingPathNode? this[int i] => _nodes.Count > 0 ? _nodes[i] : null; 

    public FollowingPath(int maxNodes = 128)
    {
        _nodes = new List<FollowingPathNode>(128);
        MaxNodes = maxNodes;
    }

    public void Clear()
    {
        _nodes.Clear();
    }
    
    public void AddNode(Vector2 position, float radius = 32f, float arrivalRadius = 36f)
    {
        if (NodeCount < MaxNodes)
        {
            FollowingPathNode pathNode = new FollowingPathNode(position, null, radius, arrivalRadius);
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

    public FollowingPathNode? GetTargetNode()
    {
        if (NodeCount > 0)
            return _nodes[0];
        return null;
    }

    public void RemoveTargetNode()
    {
        if (NodeCount > 0)
            _nodes.Remove(GetTargetNode()!);
    }


    public IEnumerator<FollowingPathNode> GetEnumerator()
    {
        return _nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}