
namespace FantasyNameGenerator.Lib.Infrastructure
{
    public interface IDataLoader
    {
        Task<string> ReadFileAsync(string relativePath);
        Task<IEnumerable<string>> ReadLinesAsync(string relativePath);
        Task<T?> ReadJsonFileAsync<T>(string relativePath);
    }
}