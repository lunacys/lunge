using System;
using lunge.Library.Collisions;
using Microsoft.Xna.Framework;

namespace lunge.Library
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
        public static float InBetween(float width, float currentValue, float minValue, float maxValue)
        {
            return width * (currentValue / (maxValue - minValue));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> from specified <paramref name="angle"/> and <paramref name="magnitude"/>
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <param name="magnitude">Magnitude</param>
        /// <returns>A new <see cref="Vector2"/></returns>
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

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

        public static bool DoesLineIntersectCircle(Vector2 ahead, Vector2 ahead2, Obstacle circle)
        {
            return DoesLineIntersectCircle(ahead, ahead2, circle.Center, circle.Radius);
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
        public static float Normalize(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
    }
}
