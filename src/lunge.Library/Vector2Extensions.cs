using System;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library
{
    /// <summary>
    /// <see cref="Vector2"/> extensions
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns a normalized Vector2
        /// </summary>
        /// <param name="vec">Current <see cref="Vector2"/> value</param>
        /// <returns>Normalized <see cref="Vector2"/> value</returns>
        public static Vector2 Normalized(this Vector2 vec)
        {
            vec.Normalize();
            return vec;
        }

        /// <summary>
        /// Converts <see cref="Vector2"/> to angle in radians
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/> to be converted</param>
        /// <returns>Angle in radians</returns>
        public static float ConvertToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static void NormalizeOrZero(ref this Vector2 vec)
        {
            var rcp = 1.0f / vec.Length();
            if (float.IsFinite(rcp) && rcp > 0.0f)
                vec *= rcp;
            else
                vec *= 0.0f;
        }

        public static Vector2 NormalizedOrZero(this Vector2 vec)
        {
            vec.NormalizeOrZero();
            return vec;
        }

        public static bool IsNormalized(this Vector2 vec)
        {
            return MathF.Abs(vec.LengthSquared() - 1.0f) <= 1e-4;
        }

        public static Vector2 Abs(this Vector2 vec2)
        {
            vec2.X = Math.Abs(vec2.X);
            vec2.Y = Math.Abs(vec2.Y);
            return vec2;
        }
        
        public static Vector2 ClampToRectangleF(this Vector2 vec, RectangleF rect)
        {
            var result = vec;

            if (result.X < rect.X)
                result.X = rect.X;
            if (result.X > rect.X + rect.Width)
                result.X = rect.X + rect.Width;
            if (result.Y < rect.Y)
                result.Y = rect.Y;
            if (result.Y > rect.Y + rect.Height)
                result.Y = rect.Y + rect.Height;

            return result;
        }
    }
}