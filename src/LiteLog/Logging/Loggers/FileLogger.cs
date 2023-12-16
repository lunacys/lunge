namespace LiteLog.Logging.Loggers
{
    public class FileLogger : ILoggerFrontend
    {
        /// <summary>
        /// Gets or sets directory in which the logs will be written.
        /// It is combined with FileFormat.
        /// </summary>
        public string FileDirectory
        {
            get => _fileDirectory;
            set
            {
                _fileDirectory = value;
                UpdatePaths();
            }
        }

        /// <summary>
        /// Gets or sets file format.
        /// Is is combined with FileDirectory.
        /// <example>"log-{DateTime.Now:yyyy-MM-dd}.log"</example>
        /// </summary>
        public string FileFormat
        {
            get => _fileFormat;
            set
            {
                _fileFormat = value;
                UpdatePaths();
            }
        }

        public string FilePath { get; private set; } = null!;

        private string _fileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private string _fileFormat = $"log-{DateTime.Now:yyyy-MM-dd}.log";

        public FileLogger()
        {
            UpdatePaths();
        }

        public void Log(string message, LogLevel level)
        {
            using var sw = new StreamWriter(FilePath, true);
            sw.WriteLine(message);
        }

        private void UpdatePaths()
        {
            FilePath = Path.Combine(_fileDirectory, _fileFormat);
            
            Directory.CreateDirectory(_fileDirectory);
        }
    }
}
