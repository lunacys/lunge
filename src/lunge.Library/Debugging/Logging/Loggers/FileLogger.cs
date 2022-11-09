using System;
using System.IO;

namespace lunge.Library.Debugging.Logging.Loggers
{
    public class FileLogger : ILoggerFrontend
    {
        public string FileDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public string FilePath { get; }

        public FileLogger(string? filePath = null)
        {
            if (filePath == null)
                FilePath = Path.Combine(FileDir,
                    $"log-{DateTime.Now:yyyy-MM-dd}.log");
            else
                FilePath = filePath;

            if (!Directory.Exists(FileDir))
                Directory.CreateDirectory(FileDir);
        }

        public void Log(string message, LogLevel level)
        {
            using (var sw = new StreamWriter(FilePath, true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
