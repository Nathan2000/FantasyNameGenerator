using FantasyNameGenerator.Lib.Domain.Model;
using FantasyNameGenerator.Lib.Domain.Services;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class ManifestNameCultureProvider(IDataLoader dataLoader) : INameCultureProvider
    {
        private const string ManifestFileName = "manifest.json";
        private const string MetadataFileName = "metadata.json";

        private readonly NameComponentMapper _mapper = new(dataLoader);

        public async Task<IReadOnlyDictionary<string, NameCategory>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            string manifestPath = $"Data/{ManifestFileName}";
            var categoryDtos = await dataLoader.ReadJsonFileAsync<DataManifestDto>(manifestPath, ct)
                ?? throw new InvalidOperationException("Failed to load data manifest.");
            var categories = new Dictionary<string, NameCategory>();
            foreach ((string categoryName, var cultures) in categoryDtos)
            {
                categories.Add(categoryName, new NameCategory
                {
                    Name = categoryName,
                    Cultures = GetCultures(cultures, categoryName)
                });
            }
            return categories;
        }

        private static Dictionary<string, NameCulture> GetCultures(List<string> cultureDtos, string categoryName)
        {
            var cultures = new Dictionary<string, NameCulture>();
            foreach (string cultureName in cultureDtos)
            {
                cultures.Add(cultureName, new NameCulture
                {
                    Name = cultureName,
                    Category = categoryName,
                });
            }
            return cultures;
        }

        public async Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName, CancellationToken ct = default)
        {
            string metadataPath = $"Data/{categoryName}/{cultureName}/{MetadataFileName}";
            var metadata = await dataLoader.ReadJsonFileAsync<CultureMetadataDto>(metadataPath, ct)
                ?? throw new InvalidOperationException("Failed to deserialize culture metadata.");

            return new NameCultureMetadata
            {
                Name = cultureName,
                Category = categoryName,
                Description = metadata.Description,
                Template = metadata.Template,
                Components = await GetComponents(metadata.Components, $"Data/{categoryName}/{cultureName}", ct)
            };
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
