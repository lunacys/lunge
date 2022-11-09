using System;

namespace lunge.Library.Debugging.Logging
{
    public interface ILogger
    {
        event EventHandler<string> LogAdded;

        bool IsEnabled(LogLevel level);

        void Log(object message, LogLevel level);
    }

    public interface ILogger<out T> : ILogger
    { }
}