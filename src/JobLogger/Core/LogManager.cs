using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace JobLogger.Core
{
    public class LogManager
    {
        private readonly Dictionary<LoggerLevel, Action<IJobLogger>> _registerMethods;
        private readonly ConcurrentBag<IJobLogger> AllLevelsLoggers = new ConcurrentBag<IJobLogger>();
        private readonly ConcurrentBag<IJobLogger> ErrorLoggers = new ConcurrentBag<IJobLogger>();
        private readonly Dictionary<LogLevel, ConcurrentBag<IJobLogger>> Loggers;
        private readonly ConcurrentBag<IJobLogger> MessageLoggers = new ConcurrentBag<IJobLogger>();
        private readonly ConcurrentBag<IJobLogger> WarningLoggers = new ConcurrentBag<IJobLogger>();

        public bool SuppressErrorOnNotFoundLogger { get; set; } = true;

        public LogManager()
        {
            Loggers = new Dictionary<LogLevel, ConcurrentBag<IJobLogger>>
            {
                {LogLevel.Message, MessageLoggers},
                {LogLevel.Warning, WarningLoggers},
                {LogLevel.Error, ErrorLoggers}
            };
            _registerMethods = new Dictionary<LoggerLevel, Action<IJobLogger>>
            {
                {LoggerLevel.All, AllLevelsLoggers.Add},
                {LoggerLevel.Message, MessageLoggers.Add},
                {LoggerLevel.Warning, WarningLoggers.Add},
                {LoggerLevel.Error, ErrorLoggers.Add}
            };
        }

        public void LogMessage(string message)
        {
            Log(message);
        }

        public void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void Log(string message, LogLevel level = LogLevel.Message)
        {
            var loggers = Loggers[level];
            var all = loggers.Concat(AllLevelsLoggers);
            if (!all.Any() && !SuppressErrorOnNotFoundLogger) throw new InvalidOperationException("There is no registered logger for this severity");

            all.AsParallel().ForAll(l => l.LogMessage(message, level));
        }

        public void RegisterLogger(IJobLogger logger, LoggerLevel level = LoggerLevel.All)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _registerMethods[level](logger);
        }

        static LogManager()
        {
            Current = new LogManager();
        }
        public static LogManager Current { get; private set; }
    }
}