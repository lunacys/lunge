namespace lunge.Library.Debugging.Logging.Loggers;

public class TraceLogger : ILoggerFrontend
{
    private readonly object _lockObj = new();
    
    public void Log(string message, LogLevel level)
    {
        lock (_lockObj)
        {
            System.Diagnostics.Trace.WriteLine(message);    
        }
    }
}