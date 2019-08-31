using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public class GridCell
    {
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BorderColor { get; set; }
        public float BorderWidth { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            internal set
            {
                _position = value;
                BoundingRectangle = new Rectangle(Position.ToPoint(), new Point((int)Size.Width, (int)Size.Height));
            }
        }
        public Size2 Size { get; internal set; }
        public Rectangle BoundingRectangle { get; private set; }
        public SpriteFont Font { get; set; }
        public TextAlignment TextAlign { get; set; }

        private IControl _attachedControl;
        public IControl AttachedControl
        {
            get => _attachedControl;
            set
            {
                _attachedControl = value;
                _attachedControl.Position = Position;
                _attachedControl.Size = Size;
            }
        }

        internal GridCell(Vector2 position, Size2 size)
        {
            BackgroundColor = Color.White;
            ForegroundColor = Color.Black;
            BorderColor = Color.Black;
            Text = "";
            Image = null;
            BorderWidth = 1.0f;
            TextAlign = TextAlignment.Center;

            Position = position;
            Size = size;
            BoundingRectangle = new Rectangle(Position.ToPoint(), new Point((int)Size.Width, (int)Size.Height));
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Position, Size, BackgroundColor);

            if (Image != null)
            {
                spriteBatch.Draw(Image.Sprite, BoundingRectangle);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (Font == null)
                    throw new NullReferenceException("Set Font property first.");

                spriteBatch.DrawString(Font, Text, BoundingRectangle, ForegroundColor, TextAlign);
            }
            
            spriteBatch.DrawRectangle(Position, Size, BorderColor, BorderWidth);
        }
    }
}