namespace FantasyNameGenerator.Lib.Domain
{
    public interface INameCultureProvider
    {
        Task<IReadOnlyDictionary<string, NameCategory>> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<NameCultureMetadata> GetCultureMetadataAsync(string categoryName, string cultureName, CancellationToken ct = default);
    }
}
