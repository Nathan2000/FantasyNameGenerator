using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Model
{
    class CultureMetadataJson
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("template")]
        public required string Template { get; set; }

        [JsonPropertyName("components")]
        public Dictionary<string, NameComponentJson> Components { get; set; } = [];
    }

    class NameComponentJson
    {
        [JsonPropertyName("type")]
        public ComponentTypeJson Type { get; set; }

        [JsonPropertyName("male")]
        public required string MaleFilename { get; set; }

        [JsonPropertyName("female")]
        public required string FemaleFilename { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    enum ComponentTypeJson
    {
        Markov,
        Literal
    }
}
