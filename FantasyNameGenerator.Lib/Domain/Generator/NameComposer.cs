using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Model;
using System.Text.RegularExpressions;

namespace FantasyNameGenerator.Lib.Domain.Generator
{
    public class NameComposer
    {
        private readonly Dictionary<string, INameGenerator> _generators;
        private readonly string _template;
        private readonly NameGenerationOptions _options;

        public NameComposer(Dictionary<string, NameComponent> components, string template, NameGenerationOptions options)
        {
            _options = options;
            _template = template;
            _generators = components.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.CreateGenerator(options)
            );
        }

        public GeneratedName Generate()
        {
            var pattern = @"\{([^{}]+)\}";
            var matches = Regex.Matches(_template, pattern);

            var template = _template;
            foreach (Match match in matches)
            {
                var token = match.Groups[1].Value;
                string replacement = token switch
                {
                    var t when t.StartsWith("g|") => ParseGenderedToken(t, _options.Gender),
                    var t when _generators.ContainsKey(t) => _generators[t].GenerateName(),
                    _ => $"{{{token}}}" // Leave unchanged if no generator found
                };
                template = template.Replace(match.Value, replacement);
            }
            return new GeneratedName
            {
                FullName = template,
            };
        }

        private static string ParseGenderedToken(string token, Gender gender)
        {
            var parts = token.Split('|');
            if (parts.Length != 3 || parts[0] != "g")
                return $"{{{token}}}"; // Invalid format, return as is

            return gender == Gender.Male ? parts[1] : parts[2];
        }
    }
}
