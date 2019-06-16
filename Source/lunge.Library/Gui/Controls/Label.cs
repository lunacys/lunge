using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Gui.Controls
{
    public class Label : ControlBase
    {
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public TextAlignment Alignment { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }

        public Label(string name, SpriteFont font, Vector2 position, string text, Color color, TextAlignment alignment = TextAlignment.Left)
            : base(name)
        {
            Font = font;
            Position = position;
            Text = text;
            Color = color;
            Alignment = alignment;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color);

            base.Draw(spriteBatch);
        }
    }
}