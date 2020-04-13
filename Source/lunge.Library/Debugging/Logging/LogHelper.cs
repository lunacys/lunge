using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    // TODO: ADD CONTEXT (e.g. GameplayScreen, GameRoot, etc.)!
    /// <summary>
    /// Provides a logging system which might work on either sync or async.
    /// </summary>
    public class LogHelper
    {
        private static readonly List<Logger> ActiveLoggers = new List<Logger>();
        private static readonly Dictionary<string, LogHelper> LogHelpers = new Dictionary<string, LogHelper>();
        public string Context { get; }

        internal LogHelper(string context, LogTarget target)
        {
            Context = context;

            Target = target;
        }

        public static LogTarget Target
        {
            get => _target;
            set
            {
                ActiveLoggers.Clear();

                if (value.HasFlag(LogTarget.Console))
                    ActiveLoggers.Add(new ConsoleLogger());
                if (value.HasFlag(LogTarget.File))
                    ActiveLoggers.Add(new FileLogger());

                _target = value;
            }
        }
        
        private static LogTarget _target;

        public static LogHelper GetLogger(string context = "", LogTarget target = LogTarget.Console | LogTarget.File)
        {
            if (!LogHelpers.ContainsKey(context))
            {
                LogHelpers[context] = new LogHelper(context, target);
            }

            return LogHelpers[context];
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            var builtString = BuildString(message, level);

            foreach (var logger in ActiveLoggers)
                logger.Log(builtString, level);
        }

        public async Task LogAsync(string message, LogLevel level = LogLevel.Info)
        {
            var buildString = BuildString(message, level);

            foreach (var logger in ActiveLoggers)
                await logger.LogAsync(buildString, level).ConfigureAwait(false);
        }

        private string BuildString(string message, LogLevel level)
        {
            string result = "";
            string curDateTimeStr = DateTime.Now.ToString("G");

            switch (level)
            {
                case LogLevel.Info:
                    result += $"[INFO] - {curDateTimeStr} - {Context} - ";
                    break;
                case LogLevel.Warning:
                    result += $"[WARN] - {curDateTimeStr} - {Context} - ";
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
