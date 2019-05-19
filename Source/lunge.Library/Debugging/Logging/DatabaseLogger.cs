using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace lunge.Library.Debugging.Logging
{
    public class DatabaseLogger : Logger
    {
        private SQLiteConnection _dbConnection;

        public string DatabaseName
        {
            get { return "logs.db"; }
        }

        public DatabaseLogger()
        {
            _dbConnection = new SQLiteConnection($"Data Source={DatabaseName};Version=3;");
            CreateTable();
        }

        public override void Log(string message, LogLevel level)
        {
            _dbConnection.Open();

            SQLiteCommand dbInsertCommand = _dbConnection.CreateCommand();
            dbInsertCommand.CommandText = 
                $"INSERT INTO logs (log_level, log_message) VALUES (" + 
                $"'{level.ToString()}', " +
                $"'{message}');";
            dbInsertCommand.ExecuteNonQuery();

            _dbConnection.Close();
        }

        public override async Task LogAsync(string message, LogLevel level)
        {
            await _dbConnection.OpenAsync();

            SQLiteCommand dbInsertCommand = _dbConnection.CreateCommand();
            dbInsertCommand.CommandText =
                $"INSERT INTO logs (log_level, log_message) VALUES (" +
                $"'{level.ToString()}', " +
                $"'{message}');";
            await dbInsertCommand.ExecuteNonQueryAsync();

            await new TaskFactory().StartNew(() => _dbConnection.Close());
        }

        private void CreateTable()
        {
            _dbConnection.Open();

            SQLiteCommand dbCreateTableCommand = _dbConnection.CreateCommand();
            dbCreateTableCommand.CommandText =
                 @"CREATE TABLE IF NOT EXISTS
                    [logs] (                  
                    [log_level]                            VARCHAR(16)    DEFAULT NULL,               
                    [log_message]                          NVARCHAR(4096) DEFAULT NULL)";
            dbCreateTableCommand.ExecuteNonQuery();

            _dbConnection.Close();
        }
    }
}