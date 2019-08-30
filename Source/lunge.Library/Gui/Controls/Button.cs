using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public enum ButtonState
    {
        None,
        Hovered,
        Clicking,
        Pressed
    }

    public class Button : ControlBase
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }

        public event EventHandler Clicked;

        public Point Position { get; set; }
        public RectangleF BoundingRect => new RectangleF(Position, new Size2(Width, Height));

        public ButtonState State { get; private set; }

        public SpriteFont Font { get; set; }

        public Button(string name, Point position, int width, int height, SpriteFont font) 
            : base(name)
        {
            Position = position;
            Width = width;
            Height = height;
            State = ButtonState.None;
            Font = font;
        }

        public override void Update(GameTime gameTime, InputHandler inputHandler)
        {
            if (BoundingRect.Contains(inputHandler.MousePosition))
            {
                if (inputHandler.IsMouseButtonDown(MouseButton.Left))
                {
                    State = ButtonState.Clicking;
                }
                else if (inputHandler.WasMouseButtonReleased(MouseButton.Left))
                {
                    if (State == ButtonState.Clicking)
                    {
                        State = ButtonState.Pressed;

                        Clicked?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    State = ButtonState.Hovered;
                }
            }
            else
            {
                State = ButtonState.None;
            }

            base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (State)
            {
                case ButtonState.None:
                    spriteBatch.FillRectangle(BoundingRect, Color.Red);
                    break;
                case ButtonState.Hovered:
                    spriteBatch.FillRectangle(BoundingRect, Color.Green);
                    break;
                case ButtonState.Pressed:
                    spriteBatch.FillRectangle(BoundingRect, Color.Blue);
                    break;
                case ButtonState.Clicking:
                    spriteBatch.FillRectangle(BoundingRect, Color.Wheat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!string.IsNullOrEmpty(Text) && Font != null)
            {
                spriteBatch.DrawString(Font, Text, BoundingRect.ToRectangle(), Color.Black, TextAlignment.Center);
            }

            base.Draw(spriteBatch);
        }

        public override RectangleF GetBounds()
        {
            return BoundingRect;
        }
    }
}