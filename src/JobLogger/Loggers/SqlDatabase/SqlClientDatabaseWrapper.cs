using System.Data.Common;
using System.Data.SqlClient;

namespace JobLogger.Loggers.SqlDatabase
{
    public class SqlClientDatabaseWrapper : IDatabaseWrapper
    {
        public DbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
    }
}