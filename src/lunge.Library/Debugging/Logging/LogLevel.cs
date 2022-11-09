namespace lunge.Library.Debugging.Logging
{
    public enum LogLevel
    {
        Trace,
        Debug,
        /// <summary>
        /// General and non-harmous information message
        /// </summary>
        Info,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning,
        /// <summary>
        /// Error message
        /// </summary>
        Error,
        /// <summary>
        /// Critical error message that usually makes a program crash
        /// </summary>
        Critical
    }
}
