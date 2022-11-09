using System;
using System.Collections.Generic;

namespace lunge.Library.Debugging.Logging
{
    /// <summary>
    /// Provides a simple logging system.
    /// </summary>
    public class Logger : ILogger
    {
        public string Context { get; }
        private List<ILoggerFrontend> ActiveLoggers { get; }

        private Dictionary<LogLevel, ILoggerFrontend> _activeLoggersMap { get; } 

        public void AddLoggerFrontend(ILoggerFrontend logger)
        {
            if (ActiveLoggers.Contains(logger))
            {
                return;
            }

            ActiveLoggers.Add(logger);
        }

        internal Logger(string context, IEnumerable<ILoggerFrontend> activeLoggers)
        {
            Context = context;
            ActiveLoggers = new List<ILoggerFrontend>();
            ActiveLoggers.AddRange(activeLoggers);

            _activeLoggersMap = new Dictionary<LogLevel, ILoggerFrontend>();
        }

        public event EventHandler<string>? LogAdded;

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(object message, LogLevel level = LogLevel.Debug)
        {
            var builtString = BuildString(message, level);
            LogAdded?.Invoke(this, builtString);

            foreach (var logger in ActiveLoggers)
                logger.Log(builtString, level);
        }

        private string BuildString(object message, LogLevel level)
        {
            string result = "";
            string curDateTimeStr = DateTime.Now.ToString("G");

            switch (level)
            {
                case LogLevel.Trace:
                    result += $"[TRACE] - {curDateTimeStr} - {Context} - ";
                    break;
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
            
            return result + message;
        }

        
    }
}
