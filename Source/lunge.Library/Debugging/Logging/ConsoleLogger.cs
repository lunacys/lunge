using System;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public class ConsoleLogger : Logger
    {
        private ConsoleColor _origColor;
        public ConsoleColor ColorInfo { get; set; }
        public ConsoleColor ColorWarning { get; set; }
        public ConsoleColor ColorError { get; set; }
        public ConsoleColor ColorFatal { get; set; }

        public ConsoleLogger()
        {
            _origColor = Console.ForegroundColor;
            TakeDefaultColors();
        }

        protected void TakeDefaultColors()
        {
            ColorInfo = ConsoleColor.White;
            ColorWarning = ConsoleColor.DarkYellow;
            ColorError = ConsoleColor.Red;
            ColorFatal = ConsoleColor.DarkRed;
        }

        public override void Log(string message, LogLevel level)
        {
            lock (LockObject)
            {
                switch (level)
                {
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

        public override async Task LogAsync(string message, LogLevel level)
        {
            switch (level)
            {
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

            await Console.Out.WriteLineAsync(message);
            
            Console.ForegroundColor = _origColor;
        }
    }
}
