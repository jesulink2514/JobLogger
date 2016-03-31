using System;
using FakeItEasy;
using JobLogger.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobLogger.Tests
{
    [TestClass]
    public class LogManagerTest
    {
        [TestMethod,
         ExpectedException(typeof (InvalidOperationException))]
        public void Log_IfNoLoggerRegisteredAndNoSupress_ThrowInvalidOperationException()
        {
            //Arrange
            var logManager = new LogManager();
            var dummyMessage = "I'm a message";
            logManager.SuppressErrorOnNotFoundLogger = false;
            //Act
            logManager.LogMessage(dummyMessage);

            //Assert
            Assert.Fail("It should have thrown an exception");
        }

        [TestMethod]
        public void Log_IfNoLoggerRegisteredAndSupress_JustNoLog()
        {
            //Arrange
            var logManager = new LogManager();
            var dummyMessage = "I'm a message";
            //Act
            logManager.LogMessage(dummyMessage);
            logManager.LogError(dummyMessage);
            logManager.LogWarning(dummyMessage);

            //Assert            
        }

        [TestMethod,
         ExpectedException(typeof (ArgumentNullException))]
        public void RegisterLogger_IfLoggerIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var logManager = new LogManager();
            IJobLogger logger = null;

            //Act
            logManager.RegisterLogger(logger);

            //Assert
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Log_IfLevelIsMessage_OnlyCallMessageLoggers()
        {
            //Arrange
            var logManager = new LogManager();
            var logger = A.Fake<IJobLogger>();
            var noLogger = A.Fake<IJobLogger>();

            const string message = "I'm a dummy message";
            const LogLevel level = LogLevel.Message;

            //Act
            logManager.RegisterLogger(logger, LoggerLevel.Message);
            logManager.RegisterLogger(noLogger, LoggerLevel.Error);

            logManager.Log(message, LogLevel.Message);
            //Assert
            A.CallTo(() => logger.LogMessage(A<string>.Ignored, level)).MustHaveHappened();
            A.CallTo(() => noLogger.LogMessage(A<string>.Ignored, level)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Log_IfLoggerIsRegisteredWithAllLevel_LogManagerCallItForAllLevels()
        {
            //Arrange
            var logManager = new LogManager();
            var logger = A.Fake<IJobLogger>();
            const string message = "I'm a dummy message";

            //Act
            logManager.RegisterLogger(logger, LoggerLevel.All);
            logManager.Log(message, LogLevel.Error);
            logManager.Log(message, LogLevel.Warning);

            //Assert
            A.CallTo(() => logger.LogMessage(A<string>.Ignored, A<LogLevel>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Twice);
        }        
    }
}