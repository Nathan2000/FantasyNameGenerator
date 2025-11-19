using CommunityToolkit.Mvvm.Input;
using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Model;
using System.Collections.ObjectModel;

namespace FantasyNameGenerator.WPF.ViewModels.Main
{
    public class DesignMainViewModel : IMainViewModel
    {
        public ObservableCollection<NameCategory> Categories{ get; }
        public ObservableCollection<NameCulture> Cultures { get; }
        public NameCategory? SelectedCategory { get; set; }
        public NameCulture? SelectedCulture { get; set; }
        public NameCultureMetadata? Metadata { get; }
        public ObservableCollection<GeneratedName> GeneratedNames { get; }

        public Gender Gender { get; set; }
        public int SequenceSize { get; set; } = 3;
        public double LengthModifier { get; set; } = 1.0;
        public bool IsLoading { get; }

        public IAsyncRelayCommand GenerateNameCommand { get; }
        public IRelayCommand ClearNamesCommand { get; }

        public DesignMainViewModel()
        {
            Categories = [
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
            SelectedCategory = Categories.First();
            Cultures = [..SelectedCategory.Cultures.Values];
            SelectedCulture = Cultures.First();
            Metadata = new NameCultureMetadata
            {
                Name = SelectedCulture.Name,
                Category = SelectedCulture.Category,
                Description = $"{SelectedCulture.Name} names from the {SelectedCulture.Category} category.",
                Template = "{name}",
                Components = new Dictionary<string, NameComponent>
                {
                    ["name"] = new NameComponent("name", ComponentType.Literal, ["Aragorn", "Legolas", "Gimli"], ["Arwen", "Galadriel", "Eowyn"]),
                }
            };
            GeneratedNames =
            [
                new GeneratedName{ FullName = "Aragorn"},
                new GeneratedName{ FullName = "Legolas"},
                new GeneratedName{ FullName = "Gimli"},
            ];

            GenerateNameCommand = new AsyncRelayCommand(() => Task.CompletedTask);
            ClearNamesCommand = new RelayCommand(() => { });
        }
    }
}
