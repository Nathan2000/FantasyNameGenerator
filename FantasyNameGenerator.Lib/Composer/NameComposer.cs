using FantasyNameGenerator.Lib.Generator;
using FantasyNameGenerator.Lib.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FantasyNameGenerator.Lib.Composer
{
    public class NameComposer
    {
        private readonly string _template;
        private readonly Dictionary<string, INameGenerator> _generators = [];

        public NameComposer(Gender gender, int sequenceSize, CultureMetadata metadata)
        {
            _template = metadata.Template;
            foreach (var (key, component) in metadata.Components)
            {
                var generator = GetGenerator(gender, sequenceSize, component);
                _generators[key] = generator;
            }
        }

        private static INameGenerator GetGenerator(Gender gender, int sequenceSize, NameComponent component)
        {
            var names = GetGenderedNames(gender, component.MaleNames, component.FemaleNames);

            return component.Type switch
            {
                ComponentType.Markov => new MarkovGenerator(names, sequenceSize),
                ComponentType.Literal => new LiteralGenerator(names),
                _ => throw new NotSupportedException($"Component type {component.Type} is not supported.")
            };
        }

        private static string[] GetGenderedNames(Gender gender, string[] maleNames, string[] femaleNames)
        {
            return gender switch
            {
                Gender.Male => maleNames,
                Gender.Female => femaleNames,
                _ => throw new NotSupportedException($"Gender {gender} is not supported.")
            };
        }

        public string Generate()
        {
            string result = _template;
            foreach (var (key, generator) in _generators)
            {
                var name = generator.GenerateName();
                result = Regex.Replace(result, $"{{{key}}}", name);
            }
            return result;
        }
    }
}
