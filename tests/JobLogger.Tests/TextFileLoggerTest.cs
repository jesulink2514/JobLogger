using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Fakes;
using FakeItEasy;
using JobLogger.Core;
using JobLogger.Loggers.TextFile;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobLogger.Tests
{
    [TestClass]
    public class TextFileLoggerTest
    {
        [TestMethod, ExpectedException(typeof (InvalidOperationException))]
        public void Constructor_IfNotLogFileDirectory_ThrowsInvalidOperationException()
        {
            using (ShimsContext.Create())
            {
                //Arrange                              
                ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection();
                var filesystem = A.Dummy<IFileSystemWrapper>();
                var formatter = A.Dummy<ILogFormatter>();

                //Act
                var logger = new TextFileLogger(filesystem, formatter);

                //Assert
                Assert.Fail("It should have thrown an exception");
            }
        }

        [TestMethod]
        public void LogMessage_WriteToFileInLogDirectory_LogNameIncludeDate()
        {
            using (ShimsContext.Create())
            {
                //Arrange                              
                var filesystem = A.Fake<IFileSystemWrapper>();
                var formatter = A.Fake<ILogFormatter>();
                var logger = new TextFileLogger(filesystem, formatter);

                const string dummyMessage = "I'm a dummy message";
                const string expectedLog = "Error - I'm a dummy message - 2015-11-14";
                var date = new DateTime(2015, 11, 14);
                ShimDateTime.NowGet = () => date;

                A.CallTo(() => filesystem.GetCurrentDirectory()).Returns("C://");
                A.CallTo(() => filesystem.Combine(A<string[]>.Ignored))
                    .ReturnsLazily(s => string.Concat(s.Arguments.Get<string[]>(0)));
                A.CallTo(
                    () => formatter.GetFormattedLogEntry(A<string>.Ignored, A<LogLevel>.Ignored, A<DateTime>.Ignored))
                    .Returns(expectedLog);

                const string expectedFileName = "LogFile2015-11-14.txt";
                var expectedFile = "C://Logs" + expectedFileName;

                //Act
                logger.LogMessage(dummyMessage, LogLevel.Error);

                //Assert
                A.CallTo(() => filesystem.AppendAllText(expectedFile, expectedLog)).MustHaveHappened();
                A.CallTo(() => formatter.GetFormattedLogEntry(dummyMessage, LogLevel.Error, date)).MustHaveHappened();
            }
        }
    }
}