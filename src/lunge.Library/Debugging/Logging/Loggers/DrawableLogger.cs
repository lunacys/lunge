using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;

namespace lunge.Library.Debugging.Logging.Loggers
{
    public enum DrawableLoggerStyle
    {
        Overwrite,
        Append
    }

    public class DrawableLogger : Logger, ILoggerDrawable
    {
        private readonly ObservableCollection<LogEntry> _entries;

        public DrawableLoggerStyle Style { get; set; }
        public int MaxSize { get; set; }

        private string _fullLog = "";

        private SpriteFont _spriteFont;
        private Vector2 _position;
        private Color _color;

        public DrawableLogger(DrawableLoggerStyle style, SpriteFont font, Vector2 position, Color color, int maxSize = 10)
        {
            _entries = new ObservableCollection<LogEntry>();
            _entries.ItemAdded += EntriesOnItemAdded;
            MaxSize = maxSize;
            Style = style;

            _spriteFont = font;
            _position = position;
            _color = color;
        }

        public override void Log(string message, LogLevel level)
        {
            _entries.Add(new LogEntry(message, level));
        }

        public override Task LogAsync(string message, LogLevel level)
        {
            return new TaskFactory().StartNew(() => _entries.Add(new LogEntry(message, level)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_spriteFont == null || spriteBatch == null)
                return;

            switch (Style)
            {
                case DrawableLoggerStyle.Overwrite:
                    spriteBatch.DrawString(_spriteFont, _entries.LastOrDefault()?.Message ?? "", _position, _color);
                    break;
                case DrawableLoggerStyle.Append:
                    spriteBatch.DrawString(_spriteFont, _fullLog, _position, _color);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EntriesOnItemAdded(object sender, ItemEventArgs<LogEntry> e)
        {
            UpdateString(e.Item);
        }

        private void UpdateString(LogEntry entry)
        {
            _fullLog += entry.Message + "\n";

            if (_entries.Count > MaxSize)
            {
                _entries.RemoveAt(0);
            }
        }
    }
}