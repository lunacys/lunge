using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils.DelaunayAlgorithm;


public class Triangle : IEquatable<Triangle>
{
    public Vector2 A { get; set; }
    public Vector2 B { get; set; }
    public Vector2 C { get; set; }
    public bool IsBad { get; set; }

    public Triangle(Vector2 a, Vector2 b, Vector2 c)
    {
        A = a;
        B = b;
        C = c;
    }

    public bool Contains(Vector2 v)
        => Vector2.Distance(v, A) < 0.01f ||
           Vector2.Distance(v, B) < 0.01f ||
           Vector2.Distance(v, C) < 0.01;

    public bool CircumCircleContains(Vector2 v)
    {
        var ab = A.LengthSquared();
        var cd = B.LengthSquared();
        var ef = C.LengthSquared();

        float circumX = (ab * (C.Y - B.Y) + cd * (A.Y - C.Y) + ef * (B.Y - A.Y)) / (A.X * (C.Y - B.Y) + B.X * (A.Y - C.Y) + C.X * (B.Y - A.Y));
        float circumY = (ab * (C.X - B.X) + cd * (A.X - C.X) + ef * (B.X - A.X)) / (A.Y * (C.X - B.X) + B.Y * (A.X - C.X) + C.Y * (B.X - A.X));

        var circum = new Vector2(circumX / 2, circumY / 2);
        var radius = (A - circum).LengthSquared();
        var dist = (v - circum).LengthSquared();
        return dist <= radius;
    }

    public static bool operator ==(Triangle left, Triangle right)
    {
        return (left.A == right.A || left.A == right.B || left.A == right.C)
               && (left.B == right.A || left.B == right.B || left.B == right.C)
               && (left.C == right.A || left.C == right.B || left.C == right.C);
    }

    public static bool operator !=(Triangle left, Triangle right)
    {
        return !(left == right);
    }

    public bool Equals(Triangle other)
    {
        return this == other;
    }

    public override bool Equals(object obj)
    {
        if (obj is Triangle t)
        {
            return this == t;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(A, B, C);
    }
}