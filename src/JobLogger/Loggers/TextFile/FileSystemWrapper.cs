using System.IO;

namespace JobLogger.Loggers.TextFile
{
    public class FileSystemWrapper : IFileSystemWrapper
    {
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public void AppendAllText(string file, string text)
        {
            File.AppendAllText(file, text);
        }

        public string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}