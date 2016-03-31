using System;
using JobLogger.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobLogger.Tests
{
    [TestClass]
    public class LogFormatterTest
    {
        [TestMethod]
        public void GetFormattedEntry_ReturnsProperlyFormattedText()
        {
            //Arrange
            var formatter = new LogFormatter();
            var date = new DateTime(1992, 11, 14);

            const LogLevel level = LogLevel.Warning;
            const string message = "Dummy message";
            const string expected = "Warning - Dummy message - 1992-11-14";

            //Act
            var result = formatter.GetFormattedLogEntry(message, level, date);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}