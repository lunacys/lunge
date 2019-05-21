using System.Collections.Generic;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public class QueueLogger : Logger
    {
        private readonly Queue<string> _queue;

        public QueueLogger()
        {
            _queue = new Queue<string>();
        }

        public override void Log(string message, LogLevel level)
        {
            _queue.Enqueue(message);
        }

        public override Task LogAsync(string message, LogLevel level)
        {
            return new Task(() => _queue.Enqueue(message));
        }
    }
}