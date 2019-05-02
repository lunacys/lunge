using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    /// <summary>
    /// Provides a logging system which might work on either sync or async.
    /// </summary>
    public static class LogHelper
    {
        private static Logger _logger;
        private static readonly List<Logger> _activeLoggers = new List<Logger>();
        
        public static LogTarget Target
        {
            get => _target;
            set
            {
                _activeLoggers.Clear();

                if (value.HasFlag(LogTarget.Console))
                    _activeLoggers.Add(new ConsoleLogger());
                if (value.HasFlag(LogTarget.File))
                    _activeLoggers.Add(new FileLogger());
                if (value.HasFlag(LogTarget.Database))
                    _activeLoggers.Add(new DatabaseLogger());

                _target = value;
            }
        }
        
        private static LogTarget _target;

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            var builtString = BuildString(message, level);

            foreach (var logger in _activeLoggers)
                logger.Log(builtString, level);
        }

        public static async Task LogAsync(string message, LogLevel level = LogLevel.Info)
        {
            var buildString = BuildString(message, level);

            foreach (var logger in _activeLoggers)
                await logger.LogAsync(buildString, level);
        }

        private static string BuildString(string message, LogLevel level)
        {
            string result = "";
            switch (level)
            {
                case LogLevel.Info:
                    result += "[INFO]: ";
                    break;
                case LogLevel.Warning:
                    result += "[WARN]: ";
                    break;
                case LogLevel.Error:
                    result += "[ERROR]: ";
                    break;
                case LogLevel.Critical:
                    result += "[FATAL]: ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
            
            return result + message;
        }
    }
}
