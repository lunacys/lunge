using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Debugging.Logging.Loggers
{
    public class DrawableLogger : ILoggerFrontend
    {
        private Color _color;
        private float _duration;
        private float _scale;

        public DrawableLogger(Color color, float duration = 3f, float scale = 1f)
        {
            _color = color;
            _duration = duration;
            _scale = scale;
        }

        public void Log(string message, LogLevel level)
        {
            Debug.DrawText(message, _color, _duration, _scale);
        }
    }
}