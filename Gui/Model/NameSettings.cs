namespace Gui.Model
{
    public class NameSettings
    {
        public int GenerateNameCount { get; set; }
        public int SequenceSize { get; set; }
        public Gender Gender { get; set; }
        public double LengthModifier { get; set; }
        public bool AddToResults { get; set; }
        public bool ControlLength { get; set; }
        public string BeginWith { get; set; }
    }
}
