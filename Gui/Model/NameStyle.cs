namespace Gui.Model
{
    using Gui.Model.Templates;

    public class NameStyle
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public NameTemplate Template { get; set; }

        public NameStyle()
        {
        }

        public NameStyle(string name)
            : this()
        {
            this.Name = name;
        }
    }
}