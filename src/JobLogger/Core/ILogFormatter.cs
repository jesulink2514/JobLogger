using System;

namespace JobLogger.Core
{
    public interface ILogFormatter
    {
        string GetFormattedLogEntry(string message, LogLevel level, DateTime date);
    }
}