namespace JobLogger.Core
{
    public interface IJobLogger
    {
        void LogMessage(string message, LogLevel level);
    }
}