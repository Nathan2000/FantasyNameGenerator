namespace FantasyNameGenerator.Lib.Domain.Model
{
    public class NameCategory
    {
        public required string Name { get; set; }

        public Dictionary<string, NameCulture> Cultures { get; set; } = [];
    }
}
