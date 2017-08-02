namespace Gui.Model.Templates
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gui.Generator;

    public class NameOnly : NameTemplate
    {
        public string FemaleNameFormat { get; set; }
        public string MaleNameFormat { get; set; }

        private IEnumerable<string> canonNames;

        public override Dictionary<string, NameGenerator> Prepare(NameStyle style)
        {
            this.canonNames = File.ReadAllLines(string.Format("Data\\{0}{1}.txt", style.Name, this.Settings.Gender));
            var result = new Dictionary<string, NameGenerator>();
            result.Add("name", new NameGenerator(this.canonNames, this.Settings));
            return result;
        }

        public override NameData GenerateName(Dictionary<string, NameGenerator> generators)
        {
            var result = new NameData();
            var name = generators["name"].GenerateName();
            result.Name = this.FormatName(name);
            result.IsCanonical = this.canonNames.Contains(name);
            return result;
        }

        private string FormatName(string name)
        {
            switch (this.Settings.Gender)
            {
                case Gender.Male:
                    return !string.IsNullOrEmpty(this.MaleNameFormat)
                               ? string.Format(this.MaleNameFormat, name)
                               : name;
                case Gender.Female:
                    return !string.IsNullOrEmpty(this.FemaleNameFormat)
                               ? string.Format(this.FemaleNameFormat, name)
                               : name;
                default:
                    throw new Exception("There are only two genders, you dummy.");
            }
        }
    }
}
