namespace lunge.Library.Debugging.Logging
{
    public interface ILoggerFrontend
    {
        void Log(string message, LogLevel level);
    }
}