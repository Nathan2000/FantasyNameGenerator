namespace Gui.Model.Templates
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Gui.Generator;

    [XmlInclude(typeof(NameOnly))]
    [XmlInclude(typeof(FirstAndLastName))]
    [XmlInclude(typeof(GenderedLastNames))]
    public abstract class NameTemplate
    {
        public abstract Dictionary<string, NameGenerator> Prepare(NameStyle style, NameSettings settings);

        public abstract NameData GenerateName(Dictionary<string, NameGenerator> generators);
    }
}
