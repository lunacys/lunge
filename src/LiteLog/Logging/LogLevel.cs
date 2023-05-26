namespace LiteLog.Logging
{
    public enum LogLevel
    {
        None = 0,
        Trace = 1,
        Debug = 2,
        /// <summary>
        /// General and non-harmous information message
        /// </summary>
        Info = 4,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning = 8,
        /// <summary>
        /// Error message
        /// </summary>
        Error = 16,
        /// <summary>
        /// Critical error message that usually makes a program crash
        /// </summary>
        Critical = 32
    }
}
