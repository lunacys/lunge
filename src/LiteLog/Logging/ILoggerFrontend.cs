namespace LiteLog.Logging
{
    public interface ILoggerFrontend
    {
        void Log(string message, LogLevel level);
    }
}