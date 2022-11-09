using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils.DelaunayAlgorithm;

/// <summary>
/// Implements the Prim's algorithm for finding the MST (minimum spanning tree)
/// </summary>
public class Prim
{
    public class EdgeD : Edge
    {
        public float Distance { get; private set; }

        public EdgeD(Vector2 u, Vector2 v) : base(u, v)
        {
            Distance = Vector2.Distance(u, v);
        }

        public static bool operator ==(EdgeD left, EdgeD right)
        {
            return (left.U == right.U && left.V == right.V)
                   || (left.U == right.V && left.V == right.U);
        }

        public static bool operator !=(EdgeD left, EdgeD right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is EdgeD e)
            {
                return this == e;
            }

            return false;
        }

        public bool Equals(EdgeD e)
        {
            return this == e;
        }

        public override int GetHashCode()
        {
            return U.GetHashCode() ^ V.GetHashCode();
        }
    }

    public static List<EdgeD> MinimumSpanningTree(List<EdgeD> edges, Vector2 start)
    {
        HashSet<Vector2> openSet = new HashSet<Vector2>();
        HashSet<Vector2> closedSet = new HashSet<Vector2>();

        foreach (var edge in edges)
        {
            openSet.Add(edge.U);
            openSet.Add(edge.V);
        }

        closedSet.Add(start);

        var result = new List<EdgeD>();

        while (openSet.Count > 0)
        {
            var chosen = false;
            EdgeD chosenEdge = null;
            var minWeight = float.PositiveInfinity;

            foreach (var edge in edges)
            {
                var closedVertices = 0;
                if (!closedSet.Contains(edge.U)) closedVertices++;
                if (!closedSet.Contains(edge.V)) closedVertices++;
                if (closedVertices != 1) continue;


                if (edge.Distance < minWeight)
                {
                    chosenEdge = edge;
                    chosen = true;
                    minWeight = edge.Distance;
                }
            }

            if (!chosen) break;
            result.Add(chosenEdge);
            openSet.Remove(chosenEdge.U);
            openSet.Remove(chosenEdge.V);
            closedSet.Add(chosenEdge.U);
            closedSet.Add(chosenEdge.V);
        }

        return result;
    }
}