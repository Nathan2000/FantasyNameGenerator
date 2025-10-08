
namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class FileSystemDirectoryLoader : IDirectoryLoader
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
    }
}
