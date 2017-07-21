namespace Gui.Model.Templates
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gui.Generator;

    public class NameOnly : NameTemplate
    {
        private IEnumerable<string> canonNames;

        public override Dictionary<string, NameGenerator> Prepare(NameStyle style, NameSettings settings)
        {
            this.canonNames = File.ReadAllLines(string.Format("Data\\{0}{1}.txt", style.Name, settings.Gender));
            var result = new Dictionary<string, NameGenerator>();
            result.Add("name", new NameGenerator(this.canonNames, settings));
            return result;
        }

        public override NameData GenerateName(Dictionary<string, NameGenerator> generators)
        {
            var result = new NameData();
            result.Name = generators["name"].GenerateName();
            result.IsCanonical = this.canonNames.Contains(result.Name);
            return result;
        }
    }
}
