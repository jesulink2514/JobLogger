using System;

namespace JobLogger.Core
{
    public class LogFormatter : ILogFormatter
    {
        public const string DateFormat = "yyyy-M-d";

        public string GetFormattedLogEntry(string message, LogLevel level, DateTime date)
        {
            return string.Format("{0} - {1} - {2}", level, message, date.ToString(DateFormat));
        }
    }
}