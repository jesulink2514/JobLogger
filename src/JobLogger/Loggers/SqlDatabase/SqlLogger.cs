using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using JobLogger.Core;

namespace JobLogger.Loggers.SqlDatabase
{
    public class SqlLogger : IJobLogger
    {
        private readonly string _connectionString;
        private readonly IDatabaseWrapper _databaseWrapper;

        public SqlLogger() : this(new SqlClientDatabaseWrapper())
        {
        }

        public SqlLogger(IDatabaseWrapper databaseWrapper, string connectionStringKey = "LogDatabase")
        {
            var connstringSettings = ConfigurationManager.ConnectionStrings[connectionStringKey];
            if (connstringSettings == null)
                throw new InvalidOperationException(String.Format("There is not {0} Connection String configured.",
                    connectionStringKey));
            _connectionString = connstringSettings.ConnectionString;
            _databaseWrapper = databaseWrapper;
        }

        public void LogMessage(string message, LogLevel level)
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection(_connectionString))
                {
                    connection.Open();

                    var severity = (byte) level; //Intentionaly set Error=3 because it makes more sense

                    var command = _databaseWrapper.GetDbCommand(connection,
                        "Insert into Log values(@message,@level,@date)");
                    command.Parameters.Add(new SqlParameter {ParameterName = "@message", Value = message});
                    command.Parameters.Add(new SqlParameter {ParameterName = "@level", Value = severity});
                    command.Parameters.Add(new SqlParameter {ParameterName = "@date", Value = DateTime.Now});

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new DataException("An error ocurred trying to insert logEntry into database", ex);
            }
        }
    }
}