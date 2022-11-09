using Microsoft.Xna.Framework;

namespace lunge.Library;

public class Edge
{
    public Vector2 U { get; set; }
    public Vector2 V { get; set; }

    public Edge(Vector2 u, Vector2 v)
    {
        U = u;
        V = v;
    }

    public static bool operator ==(Edge left, Edge right)
    {
        return (left.U == right.U || left.U == right.V)
               && (left.V == right.U || left.V == right.V);
    }

    public static bool operator !=(Edge left, Edge right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is Edge e)
        {
            return this == e;
        }

        return false;
    }

    public bool Equals(Edge e)
    {
        return this == e;
    }

    public override int GetHashCode()
    {
        return U.GetHashCode() ^ V.GetHashCode();
    }
}