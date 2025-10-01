using System.Net.Http.Json;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class HttpClientDataLoader(HttpClient httpClient) : IDataLoader
    {
        public async Task<string> ReadFileAsync(string relativePath)
        {
            return await httpClient.GetStringAsync($"{relativePath}");
        }

        public async Task<IEnumerable<string>> ReadLinesAsync(string relativePath)
        {
            using var stream = await httpClient.GetStreamAsync($"{relativePath}");
            using var reader = new StreamReader(stream);
            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line.Trim());
                }
            }
            return lines;
        }

        public async Task<T?> ReadJsonFileAsync<T>(string relativePath)
        {
            return await httpClient.GetFromJsonAsync<T>($"{relativePath}");
        }
    }
}
