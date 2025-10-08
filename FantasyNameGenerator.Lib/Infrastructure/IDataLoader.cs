
namespace FantasyNameGenerator.Lib.Infrastructure
{
    public interface IDataLoader
    {
        Task<string> ReadFileAsync(string relativePath, CancellationToken ct = default);
        Task<IEnumerable<string>> ReadLinesAsync(string relativePath, CancellationToken ct = default);
        Task<T?> ReadJsonFileAsync<T>(string relativePath, CancellationToken ct = default);
    }
}