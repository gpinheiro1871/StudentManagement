using System.Data;

namespace StudentManagement.Domain.Infrastructure
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }

        public DbSession(string connectionString)
        {
            Connection = new System.Data.SQLite.SQLiteConnection(connectionString);

            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
