using System.Text;

namespace LiteLog.Logging
{
    /// <summary>
    /// Provides a simple logging system.
    /// </summary>
    public class Logger : ILogger
    {
        public string Context { get; }
        private List<ILoggerFrontend> ActiveLoggers { get; }

        private Dictionary<LogLevel, ILoggerFrontend> _activeLoggersMap { get; } 

        private object _lock = new object();

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
            lock (_lock)
            {
                var builtString = BuildString(message, level);
                LogAdded?.Invoke(this, builtString);

                foreach (var logger in ActiveLoggers)
                    logger.Log(builtString, level);
            }
        }

        public void Log(LogLevel level, params object[] messages)
        {
            lock (_lock)
            {
                for (int i = 0; i < messages.Length; i++)
                {
                    var msg = messages[i];
                    var builtStr = BuildString(msg, level);
                    LogAdded?.Invoke(this, builtStr);

                    foreach (var logger in ActiveLoggers)
                    {
                        logger.Log(builtStr, level);
                    }
                }
            }
        }

        private string BuildString(object message, LogLevel level)
        {
            var result = new StringBuilder();
            string curDateTimeStr = DateTime.Now.ToString("G");

            switch (level)
            {
                case LogLevel.Trace:
                    result.Append($"[TRACE] - {curDateTimeStr} - {Context} - ");
                    break;
                case LogLevel.Debug:
                    result.Append($"[DEBUG] - {curDateTimeStr} - {Context} - ");
                    break;
                case LogLevel.Info:
                    result.Append($"[INFO]  - {curDateTimeStr} - {Context} - ");
                    break;
                case LogLevel.Warning:
                    result.Append($"[WARN]  - {curDateTimeStr} - {Context} - ");
                    break;
                case LogLevel.Error:
                    result.Append($"[ERROR] - {curDateTimeStr} - {Context} - ");
                    break;
                case LogLevel.Critical:
                    result.Append($"[FATAL] - {curDateTimeStr} - {Context} - ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            result.Append(message);
            return result.ToString();
        }

        
    }
}
