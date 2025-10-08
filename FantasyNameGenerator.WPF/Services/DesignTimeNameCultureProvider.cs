using FantasyNameGenerator.Lib.Domain;

namespace FantasyNameGenerator.WPF.Services
{
    public class DesignTimeNameCultureProvider : INameCultureProvider
    {
        public Task<IReadOnlyDictionary<string, NameCategory>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            var categories = new Dictionary<string, NameCategory>
            {
                ["Fantasy"] = new NameCategory
                {
                    Name = "Fantasy",
                    Cultures = new Dictionary<string, NameCulture>
                    {
                        ["Elven"] = new NameCulture { Name = "Elven", Category = "Fantasy" },
                        ["Dwarven"] = new NameCulture { Name = "Dwarven", Category = "Fantasy" }
                    }
                },
                ["Historical"] = new NameCategory
                {
                    Name = "Historical",
                    Cultures = new Dictionary<string, NameCulture>
                    {
                        ["Roman"] = new NameCulture { Name = "Roman", Category = "Historical" },
                        ["Greek"] = new NameCulture { Name = "Greek", Category = "Historical" }
                    }
                }
            } as IReadOnlyDictionary<string, NameCategory>;
            return Task.FromResult(categories);
        }

        public Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName, CancellationToken ct = default)
        {
            var metadata = new NameCultureMetadata
            {
                Name = cultureName,
                Category = categoryName,
                Description = $"{cultureName} names from the {categoryName} category.",
                Template = "{name}",
                Components = new Dictionary<string, NameComponent>
                {
                    ["name"] = new NameComponent("name", ComponentType.Literal, ["Aragorn", "Legolas", "Gimli"], ["Arwen", "Galadriel", "Eowyn"]),
                }
            };
            return Task.FromResult(metadata);
        }
    }
}
