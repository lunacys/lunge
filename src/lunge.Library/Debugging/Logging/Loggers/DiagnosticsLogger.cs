namespace lunge.Library.Debugging.Logging.Loggers
{
    public class DiagnosticsLogger : ILoggerFrontend
    {
        public void Log(string message, LogLevel level)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}