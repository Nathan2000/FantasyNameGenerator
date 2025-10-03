namespace FantasyNameGenerator.Lib.Domain
{
    public class NameCategory
    {
        public required string Name { get; set; }

        public Dictionary<string, NameCulture> Cultures { get; set; } = [];
    }
}
