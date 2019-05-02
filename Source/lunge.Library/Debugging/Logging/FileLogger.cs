using System;
using System.IO;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public class FileLogger : Logger
    {
        public string FileDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public string FilePath => Path.Combine(FileDir,
            $"log-{DateTime.Now:yyyy-MM-dd}.txt");

        public FileLogger()
        {
            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);
        }

        public override void Log(string message, LogLevel level)
        {
            lock (LockObject)
            {
                using (var sw = new StreamWriter(FilePath, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public override async Task LogAsync(string message, LogLevel level)
        {
            using (var sw = new StreamWriter(FilePath, true))
            {
                await sw.WriteLineAsync(message);
            }
        }
    }
}
