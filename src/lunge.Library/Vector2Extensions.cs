using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

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

        public static Vector2 Abs(this Vector2 vec2)
        {
            vec2.X = Math.Abs(vec2.X);
            vec2.Y = Math.Abs(vec2.Y);
            return vec2;
        }

        public static Vector2 Copy(this Vector2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vector2 ClampToRectangleF(this Vector2 vec, RectangleF rect)
        {
            var result = vec.Copy();

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