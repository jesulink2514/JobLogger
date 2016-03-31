using System;
using System.Diagnostics;
using System.IO;
using JobLogger.Core;
using JobLogger.Loggers.TextFile;

namespace TextFileSample
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Current.RegisterLogger(new TextFileLogger());

            LogManager.Current.LogMessage("I'm a messae");
            LogManager.Current.LogError("I'm an error");
            LogManager.Current.LogWarning("I'm a warning");

            Console.WriteLine("Press enter to open logfile");
            Console.ReadLine();

            var relative = String.Format("Logs//LogFile{0:yyyy-MM-dd}.txt", DateTime.Now);
            var file = Path.Combine(Directory.GetCurrentDirectory(),relative);
            Process.Start(file);
            Console.ReadLine();

            Environment.Exit(1);
        }
    }
}
