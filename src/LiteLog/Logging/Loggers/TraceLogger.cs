namespace LiteLog.Logging.Loggers;

public class TraceLogger : ILoggerFrontend
{
    public void Log(string message, LogLevel level)
    {
        System.Diagnostics.Trace.WriteLine(message);
    }
}