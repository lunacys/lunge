using System;
using System.Collections.Generic;
using lunge.Library;
using Microsoft.Xna.Framework;
using Nez.AI.Pathfinding;

namespace Playground.Components.WorldGeneration;

public class AStarExitFinderGraph : IAstarGraph<Vector2>
{
    public IEnumerable<Edge> Nodes { get; } 

    public AStarExitFinderGraph(IEnumerable<Edge> nodes)
    {
        Nodes = nodes;
    }

    public IEnumerable<Vector2> GetNeighbors(Vector2 node)
    {
        var res = new List<Vector2>();

        foreach (var edge in Nodes)
        {
            if (edge.V == node)
                res.Add(edge.U);
            else if (edge.U == node)
                res.Add(edge.V);
        }

        return res;
    }

    public int Cost(Vector2 from, Vector2 to)
    {
        return 0;
    }

    public int Heuristic(Vector2 node, Vector2 goal)
    {
        return (int)(Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y));
    }
}