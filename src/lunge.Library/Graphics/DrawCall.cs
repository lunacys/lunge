using System;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Graphics
{
    public class DrawCall : IEquatable<DrawCall>
    {
        public SpriteBatch SpriteBatch { get; }
        public SpriteBatchSettings Settings { get; }

        public DrawCall(SpriteBatch spriteBatch, SpriteBatchSettings settings)
        {
            SpriteBatch = spriteBatch;
            Settings = settings;
        }


        public bool Equals(DrawCall other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(SpriteBatch, other.SpriteBatch) && Settings.Equals(other.Settings);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DrawCall) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SpriteBatch != null ? SpriteBatch.GetHashCode() : 0) * 397) ^ Settings.GetHashCode();
            }
        }
    }
}