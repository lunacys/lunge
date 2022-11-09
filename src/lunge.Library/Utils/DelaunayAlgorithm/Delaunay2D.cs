using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Utils.DelaunayAlgorithm;

/// <summary>
/// Creates a triangulated network between vertices. Use method <b>Triangulate</b>.
/// </summary>
public class Delaunay2D
{
    private List<Vector2> _vertices;
    private List<Edge> _edges;
    private List<Triangle> _triangles;

    public IEnumerable<Vector2> Vertices => _vertices;
    public IEnumerable<Edge> Edges => _edges;
    // public IEnumerable<Triangle> Triangles => _triangles;

    private Delaunay2D()
    {
        _edges = new List<Edge>();
        _triangles = new List<Triangle>();
    }

    public static Delaunay2D Triangulate(List<Vector2> vertices)
    {
        var result = new Delaunay2D
        {
            _vertices = new List<Vector2>(vertices)
        };
        result.Triangulate();
        return result;
    }

    public static Delaunay2D TriangulateConstraint(List<RectangleF> rects)
    {
        var result = new Delaunay2D();

        for (int i = 0; i < rects.Count; i++)
        {
            for (int j = i + 1; j < rects.Count; j++)
            {
                var a = rects[i];
                var b = rects[j];

                //if (!IntersectionHelper.IntersectsAny(rects, a, b))
                    result._edges.Add(new Edge(a.Center, b.Center));
            }
        }
        return result;
    }

    private void Triangulate()
    {
        var minX = _vertices[0].X;
        var minY = _vertices[0].Y;
        var maxX = minX;
        var maxY = minY;

        foreach (var vertex in _vertices)
        {
            if (vertex.X < minX) minX = vertex.X;
            if (vertex.X > maxX) maxX = vertex.X;
            if (vertex.Y < minY) minY = vertex.Y;
            if (vertex.Y > maxY) maxY = vertex.Y;
        }

        var dx = maxX - minX;
        var dy = maxY - minY;
        var deltaMax = MathF.Max(dx, dy) * 2;

        var p1 = new Vector2(minX - 1, minY - 1);
        var p2 = new Vector2(minX - 1, maxY + deltaMax);
        var p3 = new Vector2(maxX + deltaMax, minY - 1);

        _triangles.Add(new Triangle(p1, p2, p3));

        foreach (var vertex in _vertices)
        {
            List<Edge> polygon = new List<Edge>();

            foreach (var t in _triangles)
            {
                if (t.CircumCircleContains(vertex))
                {
                    t.IsBad = true;
                    polygon.Add(new Edge(t.A, t.B));
                    polygon.Add(new Edge(t.B, t.C));
                    polygon.Add(new Edge(t.C, t.A));
                }
            }

            _triangles.RemoveAll(t => t.IsBad);

            /*for (int i = 0; i < polygon.Count; i++)
            {
                for (int j = i + 1; j < polygon.Count; j++)
                {
                    if (Edge.AlmostEqual(polygon[i], polygon[j]))
                    {
                        polygon[i].IsBad = true;
                        polygon[j].IsBad = true;
                    }
                }
            }*/

            // polygon.RemoveAll(e => e.IsBad);

            foreach (var edge in polygon)
            {
                _triangles.Add(new Triangle(edge.U, edge.V, vertex));
            }
        }

        _triangles.RemoveAll(t => t.Contains(p1) || t.Contains(p2) || t.Contains(p3));

        HashSet<Edge> edgeSet = new HashSet<Edge>();

        foreach (var t in _triangles)
        {
            var ab = new Edge(t.A, t.B);
            var bc = new Edge(t.B, t.C);
            var ca = new Edge(t.C, t.A);

            if (edgeSet.Add(ab))
                _edges.Add(ab);
            if (edgeSet.Add(bc))
                _edges.Add(bc);
            if (edgeSet.Add(ca)) 
                _edges.Add(ca);
        }
    }

    public static bool AlmostEqual(float x, float y) =>
        MathF.Abs(x - y) <= float.Epsilon * MathF.Abs(x + y) * 2 ||
        MathF.Abs(x - y) < float.MinValue;

    public static bool AlmostEqual(Vector2 left, Vector2 right) =>
        AlmostEqual(left.X, left.Y) && AlmostEqual(right.X, right.Y);
}