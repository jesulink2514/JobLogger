using System;
using System.Collections.Generic;
using JobLogger.Core;

namespace JobLogger.Loggers.Console
{
    public class ConsoleLogger : IJobLogger
    {
        private static readonly Dictionary<LogLevel, ConsoleColor> _color = new Dictionary<LogLevel, ConsoleColor>
        {
            {LogLevel.Message, ConsoleColor.White},
            {LogLevel.Warning, ConsoleColor.Yellow},
            {LogLevel.Error, ConsoleColor.Red}
        };

        private readonly IConsoleWrapper _console;
        private readonly ILogFormatter _formatter;

        public ConsoleLogger(IConsoleWrapper console, ILogFormatter formatter)
        {
            if (console == null) throw new ArgumentNullException("console");
            if (formatter == null) throw new ArgumentNullException("formatter");
            _console = console;
            _formatter = formatter;
        }

        public ConsoleLogger() : this(new ConsoleWrapper(), new LogFormatter())
        {
        }

        public void LogMessage(string message, LogLevel level)
        {
            _console.ForegroundColor = _color[level];
            _console.WriteLine(_formatter.GetFormattedLogEntry(message, level, DateTime.Now));
        }
    }
}