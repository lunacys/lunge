using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public class Tooltip : ControlBase
    {
        // TODO: Add Color
        // TODO: Add showing delay
        public string Text { get; set; }
        public Vector2 Position { get; private set; }
        public Size2 Size { get; set; }
        public bool IsVisible { get; private set; }
        public SpriteFont Font { get; }

        public Tooltip(string name, IControl parentControl, SpriteFont font)
            : base(name, parentControl)
        {
            Font = font;
            DrawDepth = 0.0f; // Draw over all other content
        }

        public override void Update(GameTime gameTime, InputHandler inputHandler)
        {
            // TODO: Measure string and change size only if Text property was changed
            var stringSize = Font.MeasureString(Text);

            if (stringSize.X > Size.Width)
            {
                Size = new Size2(stringSize.X + 2, stringSize.Y);
            }

            var mousePos = inputHandler.MousePositionScreenToWorld;

            if (ParentControl.GetBounds().Contains(mousePos))
            {
                IsVisible = true;
                // TODO: Add tooltip offset
                Position = mousePos + new Vector2(24, 10);
            }
            else
            {
                IsVisible = false;
            }

            base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.FillRectangle(Position, Size, Color.White);
                spriteBatch.DrawString(Font, Text, GetBounds().ToRectangle(), Color.Black, TextAlignment.Center);
            }

            base.Draw(spriteBatch);
        }

        public override RectangleF GetBounds()
        {
            // TODO: Improve performance by changing the bounds only on size change
            return new RectangleF(Position.ToPoint(), Size);
        }
    }
}