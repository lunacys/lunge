using System;

namespace lunge.Library.Debugging.Logging.Loggers
{
    public class ConsoleLogger : ILoggerFrontend
    {
        private readonly ConsoleColor _origColor;
        public ConsoleColor ColorTrace { get; set; }
        public ConsoleColor ColorDebug { get; set; }
        public ConsoleColor ColorInfo { get; set; }
        public ConsoleColor ColorWarning { get; set; }
        public ConsoleColor ColorError { get; set; }
        public ConsoleColor ColorFatal { get; set; }

        private readonly object _lockObject = new object();

        public ConsoleLogger()
        {
            _origColor = Console.ForegroundColor;
            TakeDefaultColors();
        }

        protected void TakeDefaultColors()
        {
            ColorTrace = ConsoleColor.Yellow;
            ColorDebug = ConsoleColor.Green;
            ColorInfo = ConsoleColor.White;
            ColorWarning = ConsoleColor.DarkYellow;
            ColorError = ConsoleColor.Red;
            ColorFatal = ConsoleColor.DarkRed;
        }

        public void Log(string message, LogLevel level)
        {
            lock (_lockObject)
            {
                switch (level)
                {
                    case LogLevel.Trace:
                        Console.ForegroundColor = ColorTrace;
                        break;
                    case LogLevel.Debug:
                        Console.ForegroundColor = ColorDebug;
                        break;
                    case LogLevel.Info:
                        Console.ForegroundColor = ColorInfo;
                        break;
                    case LogLevel.Warning:
                        Console.ForegroundColor = ColorWarning;
                        break;
                    case LogLevel.Error:
                        Console.ForegroundColor = ColorError;
                        break;
                    case LogLevel.Critical:
                        Console.ForegroundColor = ColorFatal;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }

                Console.WriteLine(message);

                Console.ForegroundColor = _origColor;
            }
        }
    }
}
