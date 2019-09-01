using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

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
        public string Text { get; set; }

        public event EventHandler Clicked;

        public RectangleF BoundingRect => new RectangleF(Position, Size);

        public ButtonState State { get; private set; }

        public SpriteFont Font { get; set; }
        public TextureAtlas TextureAtlas { get; set; }

        public Button(string name, Vector2 position, int width, int height, SpriteFont font) 
            : base(name)
        {
            Position = position;

            Size = new Size2(width, height);
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
            if (TextureAtlas == null)
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
            }
            else
            {
                spriteBatch.DrawRectangle(GetBounds(), Color.Black, 1f);
                DrawAtlas(spriteBatch);
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

        private void DrawAtlas(SpriteBatch spriteBatch)
        {
            // TODO: Refactor & Optimize this shit
            var center = TextureAtlas["Center"];
            // Edges
            var topLeft = TextureAtlas["TopLeft"];
            var bottomLeft = TextureAtlas["BottomLeft"];
            var topRight = TextureAtlas["TopRight"];
            var bottomRight = TextureAtlas["BottomRight"];
            //Sides
            var top = TextureAtlas["Top"];
            var bottom = TextureAtlas["Bottom"];
            var left = TextureAtlas["Left"];
            var right = TextureAtlas["Right"];
            // Positions
            var topLeftPosition = Position;
            var bottomLeftPosition = new Vector2(Position.X, Position.Y + Size.Height - bottomLeft.Height);
            var topRightPosition = new Vector2(Position.X + Size.Width - topRight.Width, Position.Y);
            var bottomRightPosition = new Vector2(Position.X + Size.Width - bottomRight.Width,
                Position.Y + Size.Height - bottomRight.Height);

            // Draw Center
            {
                var rect = new Rectangle(
                    x:      (int)(topLeftPosition.X + topLeft.Width),
                    y:      (int)(topLeftPosition.Y + topLeft.Height),
                    width:  (int)(topRightPosition.X - (topLeftPosition.X + topLeft.Width)), 
                    height: (int)(bottomRightPosition.Y - (topLeftPosition.Y + topLeft.Height))
                    );
                spriteBatch.Draw(center.Texture, rect, center.Bounds, Color.White);
            }

            // Draw Sides:
            // Top
            {
                var rect = new Rectangle(
                    x:      (int)(topLeftPosition.X + topLeft.Width), 
                    y:      (int)(topLeftPosition.Y),
                    width:  (int)(topRightPosition.X - (topLeftPosition.X + topLeft.Width)), 
                    height: top.Height
                    );
                spriteBatch.Draw(top.Texture, rect, top.Bounds, Color.White);
            }
            // Bottom
            {
                var rect = new Rectangle(
                    x:      (int)(bottomLeftPosition.X + bottomLeft.Width), 
                    y:      (int)(bottomLeftPosition.Y),
                    width:  (int)(bottomRightPosition.X - (bottomLeftPosition.X + bottomLeft.Width)), 
                    height: (int)top.Height
                    );
                spriteBatch.Draw(bottom.Texture, rect, bottom.Bounds, Color.White);
            }
            // Left
            {
                var rect = new Rectangle(
                    x:      (int)(topLeftPosition.X),
                    y:      (int)(topLeftPosition.Y + topLeft.Height),
                    width:  (int)(left.Width),
                    height: (int)(bottomLeftPosition.Y - (topLeftPosition.Y + topLeft.Height))
                    );
                spriteBatch.Draw(left.Texture, rect, left.Bounds, Color.White);
            }
            // Right
            {
                var rect = new Rectangle(
                    x:      (int)(topRightPosition.X),
                    y:      (int)(topRightPosition.Y + topRight.Height),
                    width:  (int)(right.Width),
                    height: (int)(bottomRightPosition.Y - (topRightPosition.Y + topRight.Height))
                );
                spriteBatch.Draw(right.Texture, rect, right.Bounds, Color.White);
            }

            // Draw Edges
            spriteBatch.Draw(topLeft.Texture, topLeftPosition, topLeft.Bounds, Color.White);
            spriteBatch.Draw(bottomLeft.Texture, bottomLeftPosition, bottomLeft.Bounds, Color.White);
            spriteBatch.Draw(topRight.Texture, topRightPosition, topRight.Bounds, Color.White);
            spriteBatch.Draw(bottomRight.Texture, bottomRightPosition, bottomRight.Bounds, Color.White);
        }
    }
}