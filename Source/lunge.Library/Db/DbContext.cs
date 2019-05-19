using System;
using System.Data.SQLite;

namespace lunge.Library.Db
{
    public class DbContext : IDisposable
    {
        private SQLiteConnection _dbConnection;

        public DbContext(string connectionString)
        {
            _dbConnection = new SQLiteConnection(connectionString);
            _dbConnection.Open();
        }


        private void ReleaseUnmanagedResources()
        {
            _dbConnection.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _dbConnection.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DbContext()
        {
            Dispose(false);
        }
    }
}