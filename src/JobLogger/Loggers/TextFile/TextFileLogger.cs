using System;
using System.Configuration;
using JobLogger.Core;

namespace JobLogger.Loggers.TextFile
{
    public class TextFileLogger : IJobLogger
    {
        private readonly string _directory;
        private readonly IFileSystemWrapper _fileSystemWrapper;
        private readonly ILogFormatter _formatter;

        public TextFileLogger() : this(new FileSystemWrapper(), new LogFormatter())
        {
        }

        public TextFileLogger(IFileSystemWrapper fileSystemWrapper, ILogFormatter formatter)
        {
            var dir = ConfigurationManager.AppSettings["LogFileDirectory"];
            if (String.IsNullOrWhiteSpace(dir)) throw new InvalidOperationException("LogFileDirectory is not valid.");
            _fileSystemWrapper = fileSystemWrapper;
            _formatter = formatter;
            _directory = dir;
        }

        public void LogMessage(string message, LogLevel level)
        {
            var log = _formatter.GetFormattedLogEntry(message, level, DateTime.Now);
            var file = string.Format("LogFile{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
            var fullFilePath = _fileSystemWrapper.Combine(_fileSystemWrapper.GetCurrentDirectory(), _directory, file);
            _fileSystemWrapper.AppendAllText(fullFilePath, log);
        }        
    }
}