using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLogger.Core;
using JobLogger.Loggers.SqlDatabase;

namespace SqlLoggerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Current.RegisterLogger(new SqlLogger());

            LogManager.Current.LogMessage("I'm a messae");
            LogManager.Current.LogError("I'm an error");
            LogManager.Current.LogWarning("I'm a warning");

            Environment.Exit(1);
        }
    }
}
