using System;

namespace JobLogger.Loggers.Console
{
    public interface IConsoleWrapper
    {
        ConsoleColor ForegroundColor { get; set; }
        void WriteLine(string message);
    }
}