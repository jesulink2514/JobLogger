using System.Data.Common;

namespace JobLogger.Loggers.SqlDatabase
{
    public interface IDatabaseWrapper
    {
        DbConnection GetConnection(string connectionString);
        DbCommand GetDbCommand(DbConnection connection, string commandText);
    }
}