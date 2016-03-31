using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLogger.Core;
using JobLogger.Loggers.Console;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. Set up the Logger
            LogManager.Current.RegisterLogger(new ConsoleLogger(),LoggerLevel.All);

            //2. Use the logger
            LogManager.Current.LogError("Its an error");

            LogManager.Current.LogWarning("Its a warning");

            LogManager.Current.LogMessage("Its just a message");

            Console.ReadLine();

            Environment.Exit(1);
        }
    }
}
