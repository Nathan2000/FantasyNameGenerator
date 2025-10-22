namespace FantasyNameGenerator.Lib.Infrastructure
{
    public interface IDirectoryLoader
    {
        IEnumerable<string> GetDirectories(string path);
        bool DirectoryExists(string path);
    }
}
