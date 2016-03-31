using System;

namespace JobLogger.Loggers.Console
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public ConsoleColor ForegroundColor
        {
            get { return System.Console.ForegroundColor; }

            set { System.Console.ForegroundColor = value; }
        }

        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}