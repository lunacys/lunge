namespace LiteLog.Logging.Loggers;

public class MemoryLogger : ILoggerFrontend
{
    private static readonly List<string> _messages = new ();

    public static IEnumerable<string> Messages => _messages;

    public static event EventHandler? LogAdded; 

    public void Log(string message, LogLevel level)
    {
        _messages.Add(message);
        LogAdded?.Invoke(this, EventArgs.Empty);
    }
}