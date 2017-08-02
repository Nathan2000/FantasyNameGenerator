namespace Gui.Model.Templates
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gui.Generator;

    public class FirstAndLastName : NameTemplate
    {
        public string FemaleNameFormat { get; set; }
        public string MaleNameFormat { get; set; }
        public string FemaleLastNameFormat { get; set; }
        public string MaleLastNameFormat { get; set; }

        private IEnumerable<string> canonNames;
        private IEnumerable<string> canonLastNames;

        public override Dictionary<string, NameGenerator> Prepare(NameStyle style)
        {
            this.canonNames = File.ReadAllLines(string.Format("Data\\{0}{1}.txt", style.Name, this.Settings.Gender));
            this.canonLastNames = File.ReadAllLines(string.Format("Data\\{0}{1}.txt", style.Name, "LastName"));
            var result = new Dictionary<string, NameGenerator>();
            result.Add("name", new NameGenerator(this.canonNames, this.Settings));
            result.Add("lastName", new NameGenerator(this.canonLastNames, this.Settings));
            return result;
        }

        public override NameData GenerateName(Dictionary<string, NameGenerator> generators)
        {
            var name = generators["name"].GenerateName();
            var lastName = generators["lastName"].GenerateName();
            var result = new NameData();
            result.Name = string.Format("{0} {1}", this.FormatName(name), this.FormatLastName(lastName));
            result.IsCanonical = this.canonNames.Contains(name) || this.canonLastNames.Contains(lastName);
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

        private string FormatLastName(string name)
        {
            switch (this.Settings.Gender)
            {
                case Gender.Male:
                    return !string.IsNullOrEmpty(this.MaleLastNameFormat)
                               ? string.Format(this.MaleLastNameFormat, name)
                               : name;
                case Gender.Female:
                    return !string.IsNullOrEmpty(this.FemaleLastNameFormat)
                               ? string.Format(this.FemaleLastNameFormat, name)
                               : name;
                default:
                    throw new Exception("There are only two genders, you dummy.");
            }
        }
    }
}
