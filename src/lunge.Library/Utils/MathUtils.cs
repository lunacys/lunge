using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils
{
    /// <summary>
    /// Implements some useful mathematical functions
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Finds the intermediate value between <paramref name="minValue"/> and <paramref name="maxValue"/>
        /// with specified <paramref name="width"/> and <paramref name="currentValue"/>.
        /// </summary>
        /// <param name="width">Width or length of the value</param>
        /// <param name="currentValue">Current value</param>
        /// <param name="minValue">Minimal value</param>
        /// <param name="maxValue">Maximal value</param>
        /// <returns>The intermediate value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NormalizeInRange(float width, float currentValue, float minValue, float maxValue)
        {
            return width * (currentValue / (maxValue - minValue));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> from specified <paramref name="angle"/> and <paramref name="magnitude"/>
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <param name="magnitude">Magnitude</param>
        /// <returns>A new <see cref="Vector2"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 FromPolar(float angle, float magnitude, Vector2 positionFrom)
        {
            return new Vector2(magnitude * (float)Math.Cos(angle) + positionFrom.X, magnitude * (float)Math.Sin(angle) + positionFrom.Y);
        }

        public static Vector2 SetAngle(Vector2 vec, float value)
        {
            var len = vec.Length();
            vec.X = (float)Math.Cos(value) * len;
            vec.Y = (float)Math.Sin(value) * len;
            return vec;
        }

        public static float DistanceBetween2(Vector2 a, Vector2 b)
        {
            return (float)
                Math.Sqrt((a.X - b.X) * (a.X - b.X) +
                          (a.Y - b.Y) * (a.Y - b.Y));
        }

        public static bool DoesLineIntersectCircle(Vector2 ahead, Vector2 ahead2, Vector2 circleCenter, float circleRadius)
        {
            return DistanceBetween2(circleCenter, ahead) <= circleRadius ||
                   DistanceBetween2(circleCenter, ahead2) <= circleRadius;
        }

        public static int GetPolarity(float currentAngle, float angleError)
        {
            /*if (Math.Abs(currentAngle) < angleError)
                return 1;
            if (Math.Abs(currentAngle - Math.PI / 4) < angleError)
                return 2;
            if (Math.Abs(currentAngle - Math.PI / 2) < angleError)
                return 3;
            if (Math.Abs(currentAngle - 3 * Math.PI / 4) < angleError)
                return 4;
            if (Math.Abs(currentAngle - Math.PI) < angleError)
                return 5;
            if (currentAngle <= -3 * Math.PI / 4 + angleError)
                return 6;
            if (Math.Abs(currentAngle - -Math.PI / 2) < angleError)
                return 7;
            if (Math.Abs(currentAngle - (-Math.PI / 4)) < angleError)
                return 8;*/
            if (Math.Abs(currentAngle) < angleError)
                return 1;
            if (Math.Abs(currentAngle - Math.PI / 2) < angleError)
                return 2;
            if (Math.Abs(currentAngle - Math.PI) < angleError)
                return 3;
            if (currentAngle <= -Math.PI / 2 + angleError)
                return 4;

            return 0;
        }

        /// <summary>
        /// Smooth rotation based on the current <paramref name="position"/>
        /// </summary>
        /// <param name="position">Current position of the object</param>
        /// <param name="faceThis"><see cref="Vector2"/> position that the object should face</param>
        /// <param name="currentAngle">Current angle of the object in radians</param>
        /// <param name="turnSpeed">Rotation speed</param>
        /// <returns>A new angle in radians</returns>
        public static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            float desiredAngle = (float)Math.Atan2(y, x);

            float difference = MathHelper.WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return MathHelper.WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Truncates a <paramref name="vector"/> with specified <paramref name="max"/> value
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> that should be truncated</param>
        /// <param name="max">Maximal value</param>
        /// <returns></returns>
        public static Vector2 Truncate(Vector2 vector, float max)
        {
            if (vector.Length() > max)
            {
                vector.Normalize();
                if (float.IsNaN(vector.X))
                    vector.X = 0;
                if (float.IsNaN(vector.Y))
                    vector.Y = 0;

                return vector * max;
            }
            if (float.IsNaN(vector.X))
                vector.X = 0;
            if (float.IsNaN(vector.Y))
                vector.Y = 0;

            return vector;
        }

        /// <summary>
        /// Calculates distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceBetween(Vector2 a, Vector2 b)
        {
            return (float)(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
        }

        /// <summary>
        /// Normalizes value to a range between 0 and 1 from current value based on minimum and maximum of that value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="min">Minimum value possible</param>
        /// <param name="max">Maximum value possible</param>
        /// <returns>Normalized value in range [0;1]</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Snaps specified floating point value to a grid with specified size.
        /// Note: this version of the function tends to round to the lowest value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SnapToGridRounding(float value, float gridSize)
        {
            return value - value % gridSize;
        }

        /// <summary>
        /// Snaps specified vector value to a grid with specified size.
        /// Note: this version of the function tends to round to the lowest value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static Vector2 SnapToGridRounding(Vector2 value, Vector2 gridSize)
        {
            return SnapToGridRounding(value, gridSize.X, gridSize.Y);
        }
        
        public static Vector2 SnapToGridRounding(Vector2 value, float gridWidth, float gridHeight)
        {
            return new Vector2(value.X - value.X % gridWidth, value.Y - value.Y % gridHeight);
        }
        
        public static void SnapToGridRounding(ref Vector2 value, Vector2 size)
        {
            SnapToGridRounding(ref value, size.X, size.Y);
        }
        
        public static void SnapToGridRounding(ref Vector2 value, float gridWidth, float gridHeight)
        {
            value.X -= value.X % gridWidth;
            value.Y -= value.Y % gridHeight;
        }
        

        public static float SnapToGrid(float value, float gridSize)
        {
            return (float) (Math.Round(value / gridSize) * gridSize);
        }

        public static Vector2 SnapToGrid(Vector2 value, Vector2 gridSize)
        {
            return new Vector2
            (
                (float)(Math.Round(value.X / gridSize.Y) * gridSize.X),
                (float)(Math.Round(value.Y / gridSize.Y) * gridSize.X)
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Preference(Vector2 goal, Vector2 hex)
        {
            return (0xFFFF & Math.Abs((int) CrossScalar(goal, hex)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CrossScalar(Vector2 a, Vector2 b)
        {
            return (a.X * b.Y) - (a.Y * b.X);
        }

        public static IEnumerable<Point> GetPointsOnLine(Point a, Point b) =>
            GetPointsOnLine(a.X, a.Y, b.X, b.Y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotOutOfBounds(Point pos, int width, int height)
            => pos.X >= 0 && pos.Y >= 0 && pos.X < width && pos.Y < height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfBounds(Point pos, int width, int height)
            => !IsNotOutOfBounds(pos, width, height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotOutOfBounds(Vector2 pos, int width, int height)
            => pos.X >= 0 && pos.Y >= 0 && pos.X < width && pos.Y < height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfBounds(Vector2 pos, int width, int height)
            => !IsNotOutOfBounds(pos, width, height);

        public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        // The algorithm is: 
        // 1) Get control points
        // 2) Generate curve points
        public static Vector2[] GetControlsPoints(Vector2 p0, Vector2 p1, Vector2 p2, float tension = 0.5f)
        {
            // get length of lines [p0-p1] and [p1-p2]
            float d01 = Vector2.Distance(p0, p1);
            float d12 = Vector2.Distance(p1, p2);
            // calculate scaling factors as fractions of total
            float sa = tension * d01 / (d01 + d12);
            float sb = tension * d12 / (d01 + d12);
            // left control point
            float c1x = p1.X - sa * (p2.X - p0.X);
            float c1y = p1.Y - sa * (p2.Y - p0.Y);
            // right control point
            float c2x = p1.X + sb * (p2.X - p0.X);
            float c2y = p1.Y + sb * (p2.Y - p0.Y);
            // return control points
            return new Vector2[] { new Vector2(c1x, c1y), new Vector2(c2x, c2y) };
        }

        public static List<Vector2> GenerateControlPoints(List<Vector2> knots, float tension = 0.5f)
        {
            if (knots == null || knots.Count < 3)
                return null;
            List<Vector2> res = new List<Vector2>();
            // First control point is same as first knot
            res.Add(knots.First());
            // generate control point pairs for each non-end knot 
            for (int i = 1; i < knots.Count - 1; ++i)
            {
                Vector2[] cps = GetControlsPoints(knots[i - 1], knots[i], knots[i + 1], tension);
                res.AddRange(cps);
            }
            // Last control points is same as last knot
            res.Add(knots.Last());
            return res;
        }

        public static Vector2 LinearInterp(Vector2 p0, Vector2 p1, float fraction)
        {
            float ix = p0.X + (p1.X - p0.X) * fraction;
            float iy = p0.Y + (p1.Y - p0.Y) * fraction;
            return new Vector2(ix, iy);
        }

        public static Vector2 BezierInterp(Vector2 p0, Vector2 p1, Vector2 c0, Vector2 c1, float fraction)
        {
            // calculate first-derivative, lines containing end-points for 2nd derivative
            var t00 = LinearInterp(p0, c0, fraction);
            var t01 = LinearInterp(c0, c1, fraction);
            var t02 = LinearInterp(c1, p1, fraction);
            // calculate second-derivate, line tangent to curve
            var t10 = LinearInterp(t00, t01, fraction);
            var t11 = LinearInterp(t01, t02, fraction);
            // return third-derivate, point on curve
            return LinearInterp(t10, t11, fraction);
        }

        public static List<Vector2> GenerateCurvePoints(List<Vector2> knots, List<Vector2> controls, int maxSteps = 20)
        {
            List<Vector2> res = new List<Vector2>();
            // start curve at first knot
            res.Add(knots[0]);
            // process each curve segment
            for (int i = 0; i < knots.Count - 1; ++i)
            {
                // get knot points for this curve segment
                Vector2 p0 = knots[i];
                Vector2 p1 = knots[i + 1];
                // get control points for this curve segment
                Vector2 c0 = controls[i * 2];
                Vector2 c1 = controls[i * 2 + 1];

                for (int s = 1; s < maxSteps; ++s)
                {
                    float fraction = (float)s / maxSteps;
                    res.Add(BezierInterp(p0, p1, c0, c1, fraction));
                }
            }
            return res;
        }
    }
}
