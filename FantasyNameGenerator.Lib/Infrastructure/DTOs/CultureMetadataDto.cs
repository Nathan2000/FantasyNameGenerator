using FantasyNameGenerator.Lib.Domain.Common;
using System.Text.Json.Serialization;

namespace FantasyNameGenerator.Lib.Infrastructure.DTOs
{
    public class CultureMetadataDto
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("template")]
        public required string Template { get; set; }
        [JsonPropertyName("components")]
        public Dictionary<string, NameComponentDto> Components { get; set; } = [];
    }

    public class NameComponentDto
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ComponentType Type { get; set; } = ComponentType.Markov;
        [JsonPropertyName("male")]
        public string? MaleFilename { get; set; }
        [JsonPropertyName("female")]
        public string? FemaleFilename { get; set; }
    }
}
