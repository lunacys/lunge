namespace lunge.Library.Debugging.Logging.Loggers
{
    public class DiagnosticsLogger : ILoggerFrontend
    {
        private readonly object _lockObj = new object();
        
        public void Log(string message, LogLevel level)
        {
            lock (_lockObj)
            {
                System.Diagnostics.Debug.WriteLine(message);    
            }
        }
    }
}