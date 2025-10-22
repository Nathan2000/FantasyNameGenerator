namespace FantasyNameGenerator.Lib.Domain.Model
{
    public class GeneratedName
    {
        public required string FullName { get; set; }
        public IReadOnlyDictionary<string, string> Components { get; set; } = new Dictionary<string, string>();

        public string GetComponent(string componentName)
        {
            if (Components.TryGetValue(componentName, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($"Component '{componentName}' not found in the generated name.");
        }
    }
}
