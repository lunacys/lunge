using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Debugging.Logging
{
    /// <summary>
    /// Provides a logging system which might work on either sync or async.
    /// </summary>
    public class LogHelper
    {
        public string Context { get; }
        private List<Logger> ActiveLoggers { get; }
        private List<ILoggerDrawable> _drawableLoggers;

        public void AddLogger(Logger logger)
        {
            if (ActiveLoggers.Contains(logger))
            {
                return;
            }

            ActiveLoggers.Add(logger);

            if (logger is ILoggerDrawable drawable)
                _drawableLoggers.Add(drawable);
        }

        internal LogHelper(string context, IEnumerable<Logger> activeLoggers)
        {
            Context = context;
            ActiveLoggers = new List<Logger>();
            ActiveLoggers.AddRange(activeLoggers);
            _drawableLoggers = new List<ILoggerDrawable>();
        }

        public void Log(object message, LogLevel level = LogLevel.Debug)
        {
            var builtString = BuildString(message, level);

            foreach (var logger in ActiveLoggers)
                logger.Log(builtString, level);
        }

        public async Task LogAsync(object message, LogLevel level = LogLevel.Debug)
        {
            var buildString = BuildString(message, level);

            foreach (var logger in ActiveLoggers)
                await logger.LogAsync(buildString, level).ConfigureAwait(false);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var logger in _drawableLoggers)
            {
                logger.Draw(spriteBatch);
            }
        }

        private string BuildString(object message, LogLevel level)
        {
            string result = "";
            string curDateTimeStr = DateTime.Now.ToString("G");

            switch (level)
            {
                case LogLevel.Debug:
                    result += $"[DEBUG] - {curDateTimeStr} - {Context} - ";
                    break;
                case LogLevel.Info:
                    result += $"[INFO]  - {curDateTimeStr} - {Context} - ";
                    break;
                case LogLevel.Warning:
                    result += $"[WARN]  - {curDateTimeStr} - {Context} - ";
                    break;
                case LogLevel.Error:
                    result += $"[ERROR] - {curDateTimeStr} - {Context} - ";
                    break;
                case LogLevel.Critical:
                    result += $"[FATAL] - {curDateTimeStr} - {Context} - ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
            
            return result + message.ToString();
        }
    }
}
