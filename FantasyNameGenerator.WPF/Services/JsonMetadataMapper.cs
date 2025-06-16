using FantasyNameGenerator.Lib.Metadata;
using FantasyNameGenerator.WPF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Services
{
    class JsonMetadataMapper
    {
        public static CultureMetadata Map(string name, string directoryPath, CultureMetadataJson metadata)
        {
            var components = new Dictionary<string, NameComponent>();
            foreach (var (key, component) in metadata.Components)
            {
                var maleNames = File.ReadAllLines(Path.Combine(directoryPath, component.MaleFilename));
                var femaleNames = File.ReadAllLines(Path.Combine(directoryPath, component.FemaleFilename));

                components.Add(key, new NameComponent
                {
                    Type = ConvertType(component.Type),
                    MaleNames = maleNames,
                    FemaleNames = femaleNames
                });
            }
            return new CultureMetadata(name, metadata.Description, metadata.Template, components);
        }

        private static ComponentType ConvertType(ComponentTypeJson type)
        {
            return type switch
            {
                ComponentTypeJson.Markov => ComponentType.Markov,
                ComponentTypeJson.Literal => ComponentType.Literal,
                _ => throw new NotSupportedException($"Component type {type} is not supported.")
            };
        }
    }
}
