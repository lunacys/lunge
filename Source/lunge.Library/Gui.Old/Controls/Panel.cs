using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Old.Controls
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
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

        public override void Update(GameTime gameTime)
        {
            if (!IsMoveable)
                return;

            if (InputManager.WasMouseButtonReleased(MouseButton.Left))
            {
                _isMoving = false;
                return;
            }

            var mousePos = InputManager.MousePosition;

            var rect = new RectangleF(Position.ToPoint(), new Size2(Size.Width, 12));

            if (rect.Contains(mousePos))
            {
                BorderColor = Color.DarkGray;

                if (InputManager.WasMouseButtonPressed(MouseButton.Left))
                {
                    _pivot = mousePos - Position;
                    _isMoving = true;
                }
                else if (InputManager.WasMouseButtonPressed(MouseButton.Right))
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