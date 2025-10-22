using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Generator;

namespace FantasyNameGenerator.Lib.Domain.Model
{
    public class NameCultureMetadata
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public required string Template { get; set; }
        public Dictionary<string, NameComponent> Components { get; set; } = [];

        private readonly Dictionary<(Gender, int), NameComposer> _composerCache = [];

        public GeneratedName GenerateName(NameGenerationOptions options)
        {
            var key = (options.Gender, options.SequenceSize);
            if (!_composerCache.TryGetValue(key, out var composer))
            {
                composer = new NameComposer(Components, Template, options);
                _composerCache[key] = composer;
            }

            return composer.Generate();
        }
    }
}
