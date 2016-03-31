using System;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data.Common;
using System.Fakes;
using FakeItEasy;
using JobLogger.Core;
using JobLogger.Loggers.SqlDatabase;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobLogger.Tests
{
    [TestClass]
    public class SqlLoggerTest
    {
        [TestMethod, ExpectedException(typeof (InvalidOperationException))]
        public void Constructor_IfConnectionStringKeyNotExits_ThrowsInvalidOperationException()
        {
            //Arrange
            var database = A.Dummy<IDatabaseWrapper>();
            var connKey = A.Dummy<string>();

            //Act
            var logger = new SqlLogger(database, connKey);

            //Assert
            Assert.Fail("Its should have thrown an exception");
        }

        [TestMethod]
        public void LogMessage_ExceuteNonQuery_PassParameters()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                ShimConfigurationManager.ConnectionStringsGet = () => new ConnectionStringSettingsCollection
                {
                    new ConnectionStringSettings("dbkey", "fake connect")
                };
                ShimDateTime.NowGet = () => new DateTime(1992, 11, 14);

                var database = A.Fake<IDatabaseWrapper>();
                var dbconn = A.Fake<DbConnection>();
                var dbcommand = A.Fake<DbCommand>();
                A.CallTo(() => database.GetDbCommand(A<DbConnection>.Ignored, A<string>.Ignored)).Returns(dbcommand);
                A.CallTo(() => database.GetConnection(A<string>.Ignored)).Returns(dbconn);

                var logger = new SqlLogger(database, "dbkey");
                var message = A.Dummy<string>();
                const LogLevel level = LogLevel.Error;

                //Act
                logger.LogMessage(message, level);

                //Assert
                A.CallTo(() => database.GetDbCommand(dbconn, A<string>.Ignored)).MustHaveHappened();
                A.CallTo(() => dbcommand.ExecuteNonQuery()).MustHaveHappened();
            }
        }
    }
}