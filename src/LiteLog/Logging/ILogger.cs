namespace LiteLog.Logging
{
    public interface ILogger
    {
        event EventHandler<string> LogAdded;

        bool IsEnabled(LogLevel level);

        void Log(object message, LogLevel level);
        void Log(LogLevel level, params object[] messages);
    }

    /*public interface ILogger<out T> : ILogger
    { }*/
}