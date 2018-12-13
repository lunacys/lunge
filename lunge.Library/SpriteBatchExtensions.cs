using System;
using lunge.Library.Entities;
using lunge.Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position,
            Color color, TextAlignment align)
        {
            Vector2 size = spriteFont.MeasureString(text);
            
            var bounds = new RectangleF(position.X, position.Y, size.X, size.Y);
            
            DrawString(spriteBatch, spriteFont, text, (Rectangle)bounds, color, align);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Rectangle bounds,
            Color color, TextAlignment align)
        {
            Vector2 size = spriteFont.MeasureString(text);
            
            Vector2 pos = new Vector2(size.X / 2, size.Y / 2);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(TextAlignment.Left))
                origin.X += bounds.Width / 2f - size.X / 2;

            if (align.HasFlag(TextAlignment.Right))
                origin.X -= bounds.Width / 2f - size.X / 2;

            if (align.HasFlag(TextAlignment.Top))
                origin.Y += bounds.Height / 2f - size.Y / 2;

            if (align.HasFlag(TextAlignment.Bottom))
                origin.Y -= bounds.Height / 2f - size.Y / 2;

            spriteBatch.DrawString(spriteFont, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position)
        {
            spriteBatch.Draw(sprite.Image, position, null, sprite.Tint, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.Effects, sprite.Depth);
        }

        public static void Begin(this SpriteBatch spriteBatch, SpriteBatchSettings spriteBatchSettings)
        {
            spriteBatch.Begin(spriteBatchSettings.SpriteSortMode, spriteBatchSettings.BlendState,
                spriteBatchSettings.SamplerState, spriteBatchSettings.DepthStencilState,
                spriteBatchSettings.RasterizerState, spriteBatchSettings.Effect, spriteBatchSettings.TransformMatrix);
        }
    }
}