namespace lunge.Library.Debugging.Logging
{

    public static class LoggerExtensions
    {
        public static void Trace(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Trace);

        public static void Debug(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Debug);

        public static void Info(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Info);

        public static void Warn(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Warning);

        public static void Critical(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Critical);

        public static void Error(this ILogger logger, object message) =>
            logger.Log(message, LogLevel.Error);

    }
}