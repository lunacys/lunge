using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace lunge.Library.Gui
{
    public interface IGuiRenderer
    {
        void Begin();

        void DrawString(SpriteFont font, string text, Vector2 position, Color color);
        void DrawString(BitmapFont font, string text, Vector2 position, Color color,
            Rectangle? clippingRectangle = null);
        void DrawRectangle(RectangleF rectangle, Color color, float thickness = 1.0f);
        void DrawRectangle(Rectangle rectangle, Color color, float thickness = 1.0f);
        void DrawRectangle(Vector2 position, Size2 size, Color color, float thickness);
        void FillRectangle(RectangleF rectangle, Color color);
        void FillRectangle(Rectangle rectangle, Color color);
        void FillRectangle(Vector2 position, Size2 size, Color color);

        void End();
    }
}