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
        public string Text { get; set; }
        public Vector2 Position { get; private set; }
        public Size2 Size { get; set; }
        public bool IsVisible { get; private set; }
        public SpriteFont Font { get; }

        public double Delay
        {
            get => _delayTimer.Interval;
            set
            {
                _delayTimer = new GameTimer(value, false, (sender, args) => { IsVisible = true; }, false);
                _isTimerSet = true;
            }
        }

        private bool _isTimerSet;
        private GameTimer _delayTimer;

        public Tooltip(string name, IControl parentControl, SpriteFont font)
            : base(name, parentControl)
        {
            Font = font;
            DrawDepth = 0.0f; // Draw over all other content
            _delayTimer = new GameTimer(0);
        }

        public override void Update(GameTime gameTime, InputHandler inputHandler)
        {
            _delayTimer.Update(gameTime);

            // TODO: Measure string and change size only if Text property was changed
            var stringSize = Font.MeasureString(Text);

            if (stringSize.X > Size.Width)
            {
                Size = new Size2(stringSize.X + 2, stringSize.Y);
            }

            var mousePos = inputHandler.MousePositionScreenToWorld;

            if (ParentControl.GetBounds().Contains(mousePos))
            {
                if (_isTimerSet && !_delayTimer.IsStarted)
                    _delayTimer.Start();
                
                if (!_isTimerSet)
                    IsVisible = true;
                // TODO: Add tooltip offset
                Position = mousePos + new Vector2(24, 10);
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