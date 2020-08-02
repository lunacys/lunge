using System;
using System.Collections.Generic;
using System.Diagnostics;
using lunge.Library.Debugging.Logging.Loggers;

namespace lunge.Library.Debugging.Logging
{
    public static class LoggerFactory
    {
        private static readonly Dictionary<string, LogHelper> LogHelpers = new Dictionary<string, LogHelper>();

        public static List<Logger> DefaultLoggers { get; set; }

        static LoggerFactory()
        {
            DefaultLoggers = new List<Logger> { new ConsoleLogger(), new FileLogger() };
        }


        public static LogHelper GetLogger()
        {
            return GetLogger(DefaultLoggers);
        }

        public static LogHelper GetLogger(List<Logger> loggers)
        {
            return GetLogger(new StackFrame(2).GetMethod()?.DeclaringType?.Name ?? "UNKNOWN", loggers);
        }

        public static LogHelper GetLogger(Type context)
        {
            return GetLogger(context.Name, DefaultLoggers);
        }

        public static LogHelper GetLogger(Type context, List<Logger> loggers)
        {
            return GetLogger(context.Name, loggers);
        }

        public static LogHelper GetLogger(string context)
        {
            return GetLogger(context, DefaultLoggers);
        }

        public static LogHelper GetLogger(string context, List<Logger> loggers)
        {
            if (!LogHelpers.ContainsKey(context))
            {
                LogHelpers[context] = new LogHelper(context, loggers);
            }

            return LogHelpers[context];
        }
    }
}