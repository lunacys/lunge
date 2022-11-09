using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Utils;

public static class IntersectionHelper
{
    public static bool IntersectsAny(List<RectangleF> rects, RectangleF a, RectangleF b)
    {
        var ray = new Ray2D(a.Center, b.Center);
        var newRects = new List<RectangleF>(rects);

        var sa = newRects.Remove(a);
        var sb = newRects.Remove(b);

        foreach (var r in newRects)
        {
            if (IntersectsRect(r, ray, out _))
                return true;
        }

        return false;
    }

    public static bool IntersectsPolygon(IEnumerable<Edge> polygon, Ray2D ray, out Vector2 intersection)
    {
        foreach (var edge in polygon)
        {
            if (IntersectsSegment(edge.U, edge.V, ray.Start, ray.End, out intersection))
                return true;
        }

        intersection = Vector2.Zero;

        return false;
    }

    public static bool IntersectsRect(RectangleF rect, Ray2D ray, out Vector2 intersection)
    {
        // a        b
        // +--------+
        // |        |
        // |        |
        // +--------+
        // d        c

        var ab = new Edge(
            new Vector2(rect.X, rect.Y),
            new Vector2(rect.X + rect.Width, rect.Y)
        );
        var bc = new Edge(
            new Vector2(rect.X + rect.Width, rect.Y),
            new Vector2(rect.X + rect.Width, rect.Y + rect.Height)
        );
        var cd = new Edge(
            new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
            new Vector2(rect.X, rect.Y + rect.Height)
        );
        var da = new Edge(
            new Vector2(rect.X, rect.Y + rect.Height),
            new Vector2(rect.X, rect.Y)
        );

        var arr = new[]
        {
            ab, bc, cd, da
        };

        return IntersectsPolygon(arr, ray, out intersection);
    }

    public static bool Intersects(this Edge edge, Ray2D ray, out Vector2 intersection)
    {
        return IntersectsSegment(edge.U, edge.V, ray.Start, ray.End, out intersection);
    }

    public static bool IntersectsSegment(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
    {
        intersection = Vector2.Zero;

        Vector2 b = a2 - a1;
        Vector2 d = b2 - b1;
        float bDotDPerp = b.X * d.Y - b.Y * d.X;

        // if b dot d == 0, it means the lines are parallel so have infinite intersection points
        if (bDotDPerp == 0)
            return false;

        Vector2 c = b1 - a1;
        float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
        if (t < 0 || t > 1)
            return false;

        float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
        if (u < 0 || u > 1)
            return false;

        intersection = a1 + t * b;

        return true;
    }
}