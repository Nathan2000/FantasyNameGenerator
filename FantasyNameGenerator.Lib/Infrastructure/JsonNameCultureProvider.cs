using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;
using System.Text.Json;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class JsonNameCultureProvider(string dataPath, IDataLoader dataLoader) : INameCultureProvider
    {
        private const string MetadataFileName = "metadata.json";
        private readonly string dataPath = dataPath ?? throw new ArgumentNullException(nameof(dataPath));

        private readonly NameComponentMapper _mapper = new(dataLoader);

        public async Task<IDictionary<string, NameCategory>> GetAllCategoriesAsync()
        {
            var categories = new Dictionary<string, NameCategory>();
            foreach (var categoryDir in Directory.GetDirectories(dataPath))
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

        public async Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName)
        {
            var metadataDir = Path.Combine(dataPath, categoryName, cultureName);
            if (string.IsNullOrEmpty(metadataDir) || !Directory.Exists(metadataDir))
                throw new DirectoryNotFoundException($"Culture directory not found: {metadataDir}");

            var metadataPath = Path.Combine(metadataDir, MetadataFileName);
            if (string.IsNullOrEmpty(metadataPath) || !File.Exists(metadataPath))
                throw new FileNotFoundException($"Culture metadata not found: {metadataPath}");

            var metadataJson = await File.ReadAllTextAsync(metadataPath);
            var metadata = JsonSerializer.Deserialize<CultureMetadataDto>(metadataJson);
            if (metadata is null)
                throw new InvalidOperationException("Failed to deserialize culture metadata.");
            return new NameCultureMetadata
            {
                Name = cultureName,
                Category = categoryName,
                Description = metadata.Description,
                Template = metadata.Template,
                Components = await GetComponents(metadata.Components, metadataDir)
            };
        }

        private static async Task<Dictionary<string, NameCulture>> GetCultures(string categoryDir)
        {
            var cultures = new Dictionary<string, NameCulture>();
            foreach (var cultureDir in Directory.GetDirectories(categoryDir))
            {
                var metadataPath = Path.Combine(cultureDir, MetadataFileName);
                if (!File.Exists(metadataPath))
                    continue; // Skip if metadata file does not exist
                var metadataJson = await File.ReadAllTextAsync(metadataPath);
                var metadata = JsonSerializer.Deserialize<CultureMetadataDto>(metadataJson);
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

        private async Task<Dictionary<string, NameComponent>> GetComponents(Dictionary<string, NameComponentDto> dtos, string cultureDirectory)
        {
            var components = new Dictionary<string, NameComponent>();
            foreach (var (componentName, dto) in dtos)
            {
                var component = await _mapper.MapAsync(componentName, dto, cultureDirectory);
                components[componentName] = component;
            }
            return components;
        }
    }
}
