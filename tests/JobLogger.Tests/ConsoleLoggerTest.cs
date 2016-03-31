using System;
using System.Fakes;
using FakeItEasy;
using JobLogger.Core;
using JobLogger.Loggers.Console;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobLogger.Tests
{
    [TestClass]
    public class ConsoleLoggerTest
    {
        [TestMethod,ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_IfNoConsoleWrapperProvided_ThrowArgumentNullException()
        {
            //Arrange            
            IConsoleWrapper console = null;
            var formatter = A.Dummy<ILogFormatter>();
            
            //Act            
            var loger = new ConsoleLogger(console, formatter);
            //Assert
            Assert.Fail("It should have thrown an exception");
        }
        private void TestColorForLevel(ConsoleColor color, LogLevel level)
        {
            //Arrange
            var console = A.Fake<IConsoleWrapper>();
            var formatter = A.Fake<ILogFormatter>();
            var loger = new ConsoleLogger(console, formatter);
            const string message = "I'm a dummy message";

            //Act                                                        
            console.ForegroundColor = ConsoleColor.Gray;
            loger.LogMessage(message, level);

            //Assert
            Assert.AreEqual(color, console.ForegroundColor);
        }

        [TestMethod]
        public void LogMessage_IfLevelIsError_SetForeColorToRed()
        {
            TestColorForLevel(ConsoleColor.Red, LogLevel.Error);
        }

        [TestMethod]
        public void LogMessage_IfLevelIsWarning_SetForeColorToYellow()
        {
            TestColorForLevel(ConsoleColor.Yellow, LogLevel.Warning);
        }

        [TestMethod]
        public void LogMessage_IfLevelIsMessage_SetForeColorToWhite()
        {
            TestColorForLevel(ConsoleColor.White, LogLevel.Message);
        }

        [TestMethod]
        public void LogMessage_WriteCurrentDateTimeAndMessage()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                var output = "";
                var console = A.Fake<IConsoleWrapper>();
                var formatter = A.Fake<ILogFormatter>();
                var loger = new ConsoleLogger(console, formatter);
                var date = new DateTime(2015, 11, 14);
                ShimDateTime.NowGet = () => date;

                const string message = "I'm a dummy message";
                const string expectedMessage = "Error - I'm a dummy message - 2015-11-14";

                A.CallTo(
                    () => formatter.GetFormattedLogEntry(A<string>.Ignored, A<LogLevel>.Ignored, A<DateTime>.Ignored))
                    .Returns(expectedMessage);
                A.CallTo(() => console.WriteLine(A<string>.Ignored)).Invokes(s => output = s.Arguments.Get<string>(0));

                //Act
                loger.LogMessage(message, LogLevel.Error);

                //Assert
                Assert.AreEqual(expectedMessage, output);
                A.CallTo(() => console.WriteLine(expectedMessage)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}