namespace JobLogger.Loggers.TextFile
{
    public interface IFileSystemWrapper
    {
        string GetCurrentDirectory();
        void AppendAllText(string file, string text);
        string Combine(params string[] paths);
    }
}