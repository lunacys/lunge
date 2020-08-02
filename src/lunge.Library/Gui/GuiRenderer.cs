using System;
using lunge.Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace lunge.Library.Gui
{
    public class GuiRenderer : IGuiRenderer
    {
        public SpriteBatchSettings Settings { get; }

        private readonly SpriteBatch _spriteBatch;

        public GuiRenderer(GraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);

            Settings = new SpriteBatchSettings();
        }

        public void Begin()
        {
            _spriteBatch.Begin(Settings);
        }

        public void End()
        {
            _spriteBatch.End();
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(font, text, position, color);
        }

        public void DrawString(BitmapFont font, string text, Vector2 position, Color color,
            Rectangle? clippingRectangle = null)
        {
            _spriteBatch.DrawString(font, text, position, color, clippingRectangle);
        }

        public void DrawRectangle(RectangleF rectangle, Color color, float thickness = 1.0f)
        {
            _spriteBatch.DrawRectangle(rectangle, color, thickness);
        }

        public void DrawRectangle(Rectangle rectangle, Color color, float thickness = 1.0f)
        {
            _spriteBatch.DrawRectangle(rectangle, color, thickness);
        }

        public void DrawRectangle(Vector2 position, Size2 size, Color color, float thickness)
        {
            _spriteBatch.DrawRectangle(position, size, color, thickness);
        }

        public void FillRectangle(RectangleF rectangle, Color color)
        {
            _spriteBatch.FillRectangle(rectangle, color);
        }

        public void FillRectangle(Rectangle rectangle, Color color)
        {
            _spriteBatch.FillRectangle(rectangle, color);
        }

        public void FillRectangle(Vector2 position, Size2 size, Color color)
        {
            _spriteBatch.FillRectangle(position, size, color);
        }
    }
}