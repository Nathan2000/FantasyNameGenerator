namespace FantasyNameGenerator.Lib.Domain.Common
{
    public class NameGenerationOptions
    {
        public Gender Gender { get; set; }
        public int SequenceSize { get; set; }
        public double LengthModifier { get; set; }

        // Add more options as needed
    }
}
