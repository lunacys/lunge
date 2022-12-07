using System;
using System.Collections.Generic;

namespace lunge.Library.Debugging.Logging.Loggers;

public class MemoryLogger : ILoggerFrontend
{
    private static readonly List<string> _messages = new ();

    public static IEnumerable<string> Messages
    {
        get
        {
            lock (_messages)
            {
                return _messages;
            }
        }   
    }

    private readonly object _lockObj = new object();

    public static event EventHandler? LogAdded; 

    public void Log(string message, LogLevel level)
    {
        lock (_lockObj)
        {
            _messages.Add(message);
            LogAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}