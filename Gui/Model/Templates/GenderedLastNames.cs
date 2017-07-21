namespace Gui.Model.Templates
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gui.Generator;

    public class GenderedLastNames : NameTemplate
    {
        private IEnumerable<string> canonNames;
        private IEnumerable<string> canonLastNames;

        public override Dictionary<string, NameGenerator> Prepare(NameStyle style, NameSettings settings)
        {
            this.canonNames = File.ReadAllLines(string.Format("Data\\{0}{1}.txt", style.Name, settings.Gender));
            this.canonLastNames = File.ReadAllLines(string.Format("Data\\{0}{1}{2}.txt", style.Name, settings.Gender, "LastName"));
            var result = new Dictionary<string, NameGenerator>();
            result.Add("name", new NameGenerator(this.canonNames, settings));
            result.Add("lastName", new NameGenerator(this.canonLastNames, settings));
            return result;
        }

        public override NameData GenerateName(Dictionary<string, NameGenerator> generators)
        {
            var name = generators["name"].GenerateName();
            var lastName = generators["lastName"].GenerateName();
            var result = new NameData();
            result.Name = string.Format("{0} {1}", name, lastName);
            result.IsCanonical = this.canonNames.Contains(name) || this.canonLastNames.Contains(lastName);
            return result;
        }
    }
}
