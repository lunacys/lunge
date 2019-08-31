using lunge.Library.GameTimers;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public class Tooltip : ControlBase
    {
        // TODO: Add Color
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _stringSize = Font?.MeasureString(_text) ?? Vector2.Zero;

                if (_stringSize.X > Size.Width || _stringSize.Y > Size.Height)
                {
                    Size = new Size2(_stringSize.X + 2, _stringSize.Y + 2);
                }
            }
        }
        public override Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _bounds.Position = _position;
            }
        }
        public override Size2 Size
        {
            get => _size;
            set
            {
                _size = value;
                _bounds = new RectangleF(Position, _size);
            }
        }
        public bool IsVisible { get; private set; }
        public SpriteFont Font { get; }
        public Vector2 Offset { get; set; } = Vector2.Zero;

        private RectangleF _bounds;

        public double Delay
        {
            get => _delayTimer.Interval;
            set
            {
                _delayTimer = new GameTimer(value, false, (sender, args) => { IsVisible = true; }, false);
                _isTimerSet = true;
            }
        }

        private string _text;
        private Vector2 _stringSize;
        private Vector2 _position;
        private Size2 _size;
        private bool _isTimerSet;
        private GameTimer _delayTimer;

        public Tooltip(string name, IControl parentControl, SpriteFont font)
            : base(name, parentControl)
        {
            Font = font;
            DrawDepth = 0.0f; // Draw over all other content
            _delayTimer = new GameTimer(0);

            _bounds = new RectangleF(Position, Size);
        }

        public override void Update(GameTime gameTime, InputHandler inputHandler)
        {
            _delayTimer.Update(gameTime);

            var mousePos = inputHandler.MousePositionScreenToWorld;

            if (ParentControl.GetBounds().Contains(mousePos))
            {
                if (_isTimerSet && !_delayTimer.IsStarted)
                    _delayTimer.Start();
                
                if (!_isTimerSet)
                    IsVisible = true;
                
                Position = mousePos + Offset;
            }
            else
            {
                if (_isTimerSet)
                    _delayTimer.Reset();
                IsVisible = false;
            }

            base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible && Font != null)
            {
                spriteBatch.FillRectangle(Position, Size, Color.White);
                spriteBatch.DrawString(Font, Text, GetBounds().ToRectangle(), Color.Black, TextAlignment.Center);
            }

            base.Draw(spriteBatch);
        }

        public override RectangleF GetBounds()
        {
            return _bounds;
        }
    }
}