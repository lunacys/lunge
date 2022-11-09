namespace lunge.Library.Debugging.Logging.Loggers;

public class TraceLogger : ILoggerFrontend
{
    public void Log(string message, LogLevel level)
    {
        System.Diagnostics.Trace.WriteLine(message);
    }
}