using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;

namespace lunge.Library;

public static class BatcherExtensions
{
    public static void DrawSprite(this Batcher batcher, Sprite sprite, Vector2 position)
    {
        batcher.Draw(sprite.Texture2D, position, sprite.SourceRect, Color.White);
    }

    public static void DrawSprite(this Batcher batcher, Sprite sprite, Vector2 position, Color color)
    {
        batcher.Draw(sprite.Texture2D, position, sprite.SourceRect, color);
    }
    
    
    public static void DrawString(this Batcher spriteBatch, IFont spriteFont, string text, Rectangle bounds,
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