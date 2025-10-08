using FantasyNameGenerator.Lib.Generator;

namespace FantasyNameGenerator.Lib.Domain
{
    public class NameComponent
    {
        public string Name { get; }
        public ComponentType Type { get; }
        public IReadOnlyList<string> MaleNames { get; } = [];
        public IReadOnlyList<string> FemaleNames { get; } = [];

        public NameComponent(
            string name,
            ComponentType type,
            IEnumerable<string> maleNames,
            IEnumerable<string> femaleNames)
        {
            Name = name;
            Type = type;
            MaleNames = [.. maleNames];
            FemaleNames = [.. femaleNames];
        }

        public INameGenerator CreateGenerator(NameGenerationOptions options)
        {
            var sourceNames = options.Gender switch
            {
                Gender.Male => MaleNames,
                Gender.Female => FemaleNames,
                _ => throw new NotSupportedException()
            };

            return Type switch
            {
                ComponentType.Markov => new MarkovGenerator([.. sourceNames], options.SequenceSize)
                {
                    LengthModifier = options.LengthModifier
                },
                ComponentType.Literal => new LiteralGenerator([.. sourceNames]),
                _ => throw new NotSupportedException($"Component type {Type} is not supported.")
            };
        }
    }
}
