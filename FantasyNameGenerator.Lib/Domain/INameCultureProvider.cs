namespace FantasyNameGenerator.Lib.Domain
{
    public interface INameCultureProvider
    {
        Task<IDictionary<string, NameCategory>> GetAllCategoriesAsync();
        Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName);
    }
}
