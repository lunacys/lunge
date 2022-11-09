using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library
{
    [Flags]
    public enum TextAlignment
    {
        Center = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }

    public static class SpriteBatchExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Rectangle bounds,
            Color color, TextAlignment align)
        {
            Vector2 size = spriteFont.MeasureString(text);
            Vector2 pos = bounds.Center.ToVector2();
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(TextAlignment.Left))
                origin.X += bounds.Width / 2.0f - size.X / 2;

            if (align.HasFlag(TextAlignment.Right))
                origin.X -= bounds.Width / 2.0f - size.X / 2;

            if (align.HasFlag(TextAlignment.Top))
                origin.Y += bounds.Height / 2.0f - size.Y / 2;

            if (align.HasFlag(TextAlignment.Bottom))
                origin.Y -= bounds.Height / 2.0f - size.Y / 2;

            spriteBatch.DrawString(spriteFont, text, pos, color, 0f, origin, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}