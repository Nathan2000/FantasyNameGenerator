namespace FantasyNameGenerator.Lib.Metadata
{
    public class CultureMetadata(string name, string? description, string template, IReadOnlyDictionary<string, NameComponent> components)
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public string Template { get; set; } = template;
        public IReadOnlyDictionary<string, NameComponent> Components { get; } = components;
    }

    public class NameComponent
    {
        public ComponentType Type { get; set; }
        public required string[] MaleNames { get; set; }
        public required string[] FemaleNames { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum ComponentType
    {
        Markov,
        Literal
    }
}
