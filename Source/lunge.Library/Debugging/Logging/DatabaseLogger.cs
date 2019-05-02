using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public class DatabaseLogger : Logger
    {
        public override void Log(string message, LogLevel level)
        {
            throw new System.NotImplementedException();
        }

        public override Task LogAsync(string message, LogLevel level)
        {
            throw new System.NotImplementedException();
        }
    }
}