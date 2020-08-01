using System;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public abstract class Logger
    {
        protected readonly object LockObject = new object();

        public abstract void Log(string message, LogLevel level);
        public abstract Task LogAsync(string message, LogLevel level);
    }
}
