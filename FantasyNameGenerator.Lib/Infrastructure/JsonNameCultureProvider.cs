using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;
using System.Text.Json;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class JsonNameCultureProvider(string dataPath, IDataLoader dataLoader, IDirectoryLoader directoryLoader) : INameCultureProvider
    {
        private const string MetadataFileName = "metadata.json";
        private readonly string dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));

        private readonly NameComponentMapper _mapper = new(dataLoader);

        public async Task<IReadOnlyDictionary<string, NameCategory>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            var categories = new Dictionary<string, NameCategory>();
            foreach (var categoryDir in directoryLoader.GetDirectories(dataPath))
            {
                var categoryName = Path.GetFileName(categoryDir);
                categories.Add(categoryName, new NameCategory
                {
                    Name = categoryName,
                    Cultures = await GetCultures(categoryDir)
                });
            }
            return categories;
        }

        public async Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName, CancellationToken ct = default)
        {
            var metadataDir = Path.Combine(dataPath, categoryName, cultureName);
            if (string.IsNullOrEmpty(metadataDir) || !directoryLoader.DirectoryExists(metadataDir))
                throw new DirectoryNotFoundException($"Culture directory not found: {metadataDir}");

            var metadataPath = Path.Combine(metadataDir, MetadataFileName);
            //if (string.IsNullOrEmpty(metadataPath) || !File.Exists(metadataPath))
            //    throw new FileNotFoundException($"Culture metadata not found: {metadataPath}");

            var metadataJson = await dataLoader.ReadFileAsync(metadataPath, ct);
            // TODO: Check if the file exists.

            var metadata = JsonSerializer.Deserialize<CultureMetadataDto>(metadataJson);
            if (metadata is null)
                throw new InvalidOperationException("Failed to deserialize culture metadata.");
            return new NameCultureMetadata
            {
                Name = cultureName,
                Category = categoryName,
                Description = metadata.Description,
                Template = metadata.Template,
                Components = await GetComponents(metadata.Components, metadataDir, ct)
            };
        }

        private async Task<Dictionary<string, NameCulture>> GetCultures(string categoryDir)
        {
            var cultures = new Dictionary<string, NameCulture>();
            foreach (var cultureDir in directoryLoader.GetDirectories(categoryDir))
            {
                var metadataPath = Path.Combine(cultureDir, MetadataFileName);
                //if (!File.Exists(metadataPath))
                //    continue;
                // TODO: Skip if metadata file does not exist
                var metadata = await dataLoader.ReadJsonFileAsync<CultureMetadataDto>(metadataPath);
                if (metadata is null)
                    continue;

                var cultureName = Path.GetFileName(cultureDir);
                cultures.Add(cultureName, new NameCulture
                {
                    Name = cultureName,
                    Category = Path.GetFileName(categoryDir)
                });
            }
            return cultures;
        }

        private async Task<Dictionary<string, NameComponent>> GetComponents(Dictionary<string, NameComponentDto> dtos, string cultureDirectory, CancellationToken ct)
        {
            var components = new Dictionary<string, NameComponent>();
            foreach (var (componentName, dto) in dtos)
            {
                var component = await _mapper.MapAsync(componentName, dto, cultureDirectory, ct);
                components[componentName] = component;
            }
            return components;
        }
    }
}
