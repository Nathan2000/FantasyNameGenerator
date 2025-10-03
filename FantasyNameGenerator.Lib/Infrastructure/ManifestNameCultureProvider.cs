using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class ManifestNameCultureProvider(IDataLoader dataLoader) : INameCultureProvider
    {
        private const string ManifestFileName = "manifest.json";
        private const string MetadataFileName = "metadata.json";

        private readonly NameComponentMapper _mapper = new(dataLoader);

        public async Task<IDictionary<string, NameCategory>> GetAllCategoriesAsync()
        {
            string manifestPath = $"Data/{ManifestFileName}";
            var categoryDtos = await dataLoader.ReadJsonFileAsync<DataManifestDto>(manifestPath)
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

        public async Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName)
        {
            string metadataPath = $"Data/{categoryName}/{cultureName}/{MetadataFileName}";
            var metadata = await dataLoader.ReadJsonFileAsync<CultureMetadataDto>(metadataPath)
                ?? throw new InvalidOperationException("Failed to deserialize culture metadata.");

            return new NameCultureMetadata
            {
                Name = cultureName,
                Category = categoryName,
                Description = metadata.Description,
                Template = metadata.Template,
                Components = await GetComponents(metadata.Components, $"Data/{categoryName}/{cultureName}")
            };
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
