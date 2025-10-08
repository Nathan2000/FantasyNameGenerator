using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.WPF.ViewModels;

namespace FantasyNameGenerator.WPF.Services
{
    internal static class DesignTimeData
    {
        public static void Set(NamesViewModel vm)
        {
            foreach (var category in GetCategories())
                vm.Categories.Add(category);

            vm.SelectedCategory = vm.Categories.FirstOrDefault();

            if (vm.SelectedCulture != null)
                vm.Metadata = GetMetadata(vm.SelectedCulture);

            foreach (var name in GetGeneratedNames())
            {
                vm.GeneratedNames.Add(name);
            }
        }

        public static List<NameCategory> GetCategories()
        {
            return
            [
                new NameCategory
                {
                    Name = "Fantasy",
                    Cultures = new Dictionary<string, NameCulture>
                    {
                        ["Elven"] = new NameCulture { Name = "Elven", Category = "Fantasy" },
                        ["Dwarven"] = new NameCulture { Name = "Dwarven", Category = "Fantasy" }
                    }
                },
                new NameCategory
                {
                    Name = "Historical",
                    Cultures = new Dictionary<string, NameCulture>
                    {
                        ["Roman"] = new NameCulture { Name = "Roman", Category = "Historical" },
                        ["Greek"] = new NameCulture { Name = "Greek", Category = "Historical" }
                    }
                }
            ];
        }

        public static NameCultureMetadata? GetMetadata(NameCulture selectedCulture)
        {
            return new NameCultureMetadata
            {
                Name = selectedCulture.Name,
                Category = selectedCulture.Category,
                Description = $"{selectedCulture.Name} names from the {selectedCulture.Category} category.",
                Template = "{name}",
                Components = new Dictionary<string, NameComponent>
                {
                    ["name"] = new NameComponent("name", ComponentType.Literal, ["Aragorn", "Legolas", "Gimli"], ["Arwen", "Galadriel", "Eowyn"]),
                }
            };
        }

        public static IEnumerable<GeneratedName> GetGeneratedNames()
        {
            return
            [
                new GeneratedName{ FullName = "Aragorn"},
                new GeneratedName{ FullName = "Legolas"},
                new GeneratedName{ FullName = "Gimli"},
            ];
        }
    }
}
