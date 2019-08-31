using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public class Panel : ControlBase
    {
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public bool IsMoveable { get; set; }
        public float BorderWidth { get; set; }

        private bool _isMoving;
        private Vector2 _pivot;

        public Panel(string name, IControl parentControl = null) 
            : base(name, parentControl)
        {
            BackgroundColor = Color.Gray;
            BorderColor = Color.Black;
            IsMoveable = false;
        }

        public override void Update(GameTime gameTime, InputHandler inputHandler)
        {
            if (!IsMoveable)
                return;

            if (inputHandler.WasMouseButtonReleased(MouseButton.Left))
            {
                _isMoving = false;
                return;
            }

            var mousePos = inputHandler.MousePositionWorldToScreen;

            var rect = new RectangleF(Position.ToPoint(), new Size2(Size.Width, 12));

            if (rect.Contains(mousePos))
            {
                BorderColor = Color.DarkGray;

                if (inputHandler.WasMouseButtonPressed(MouseButton.Left))
                {
                    _pivot = mousePos - Position;
                    _isMoving = true;
                }
                else if (inputHandler.WasMouseButtonPressed(MouseButton.Right))
                {
                    Close();
                }
            }
            else
            {
                BorderColor = Color.Black;
            }

            if (_isMoving)
            {
                BorderColor = Color.DarkGray;

                Position = mousePos - _pivot;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Position, Size, BackgroundColor);

            if (IsMoveable)
            {
                spriteBatch.FillRectangle(Position, new Size2(Size.Width, 12), BorderColor);
            }

            if (BorderWidth > 0.001f)
            {
                spriteBatch.DrawRectangle(Position, Size, BorderColor, BorderWidth);
            }
        }
    }
}