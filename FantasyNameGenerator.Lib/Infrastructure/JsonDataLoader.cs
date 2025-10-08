using System.Text.Json;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class JsonDataLoader() : IDataLoader
    {
        public async Task<string> ReadFileAsync(string relativePath, CancellationToken ct = default)
        {
            return await File.ReadAllTextAsync(relativePath, ct);
        }

        public async Task<T?> ReadJsonFileAsync<T>(string relativePath, CancellationToken ct = default)
        {
            using var json = File.OpenRead(relativePath);
            return await JsonSerializer.DeserializeAsync<T>(json, cancellationToken: ct);
        }

        public async Task<IEnumerable<string>> ReadLinesAsync(string relativePath, CancellationToken ct = default)
        {
            return await File.ReadAllLinesAsync(relativePath, ct);
        }
    }
}
