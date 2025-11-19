using CommunityToolkit.Mvvm.Input;
using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Model;
using System.Collections.ObjectModel;

namespace FantasyNameGenerator.WPF.ViewModels.Main
{
    public interface IMainViewModel
    {
        ObservableCollection<NameCategory> Categories { get; }
        ObservableCollection<NameCulture> Cultures { get; }
        NameCategory? SelectedCategory { get; }
        NameCulture? SelectedCulture { get; }
        NameCultureMetadata? Metadata { get; }
        ObservableCollection<GeneratedName> GeneratedNames { get; }
        Gender Gender { get; }
        int SequenceSize { get; }
        double LengthModifier { get; }
        bool IsLoading { get; }
        IAsyncRelayCommand GenerateNameCommand { get; }
        IRelayCommand ClearNamesCommand { get; }
    }
}
