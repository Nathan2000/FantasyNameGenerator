using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Model;
using FantasyNameGenerator.Lib.Domain.Services;
using FantasyNameGenerator.WPF.Commands;
using FantasyNameGenerator.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FantasyNameGenerator.WPF.ViewModels
{
    public class NamesViewModel : INotifyPropertyChanged
    {
        private readonly INameCultureProvider _service;

        public ObservableCollection<NameCategory> Categories { get; } = [];
        public ObservableCollection<NameCulture> Cultures { get; } = [];

        private NameCategory? _selectedCategory;
        public NameCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value)
                    return;
                _selectedCategory = value;
                OnPropertyChanged();
                Cultures.Clear();
                if (value != null)
                {
                    foreach (var culture in value.Cultures.Values)
                    {
                        Cultures.Add(culture);
                    }
                }
                SelectedCulture = Cultures.FirstOrDefault();
            }
        }

        private NameCulture? _selectedCulture;
        public NameCulture? SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                if (_selectedCulture == value)
                    return;
                _selectedCulture = value;
                OnPropertyChanged();
                _ = LoadCultureMetadataAsync(value);
            }
        }

        public ObservableCollection<GeneratedName> GeneratedNames { get; set; }

        private NameCultureMetadata? _metadata;
        public NameCultureMetadata? Metadata
        {
            get => _metadata;
            set
            {
                if (_metadata != value)
                {
                    _metadata = value;
                    OnPropertyChanged();
                }
            }
        }

        public Gender Gender { get; set; }
        public int SequenceSize { get; set; } = 3;
        public double LengthModifier { get; set; } = 1.0;

        public ICommand GenerateCommand { get; set; }
        public ICommand ClearNamesCommand { get; set; }

        public NamesViewModel() : this(ServiceLocator.NameCultureProvider) { }
        public NamesViewModel(INameCultureProvider service)
        {
            _service = service;
            GeneratedNames = [];

            GenerateCommand = new SimpleCommand(
                p => GenerateName(),
                p => SelectedCategory != null && SelectedCulture != null);
            ClearNamesCommand = new SimpleCommand(
                p => GeneratedNames?.Clear(),
                p => GeneratedNames != null && GeneratedNames.Any());

            if (ServiceLocator.IsInDesignMode)
            {
                DesignTimeData.Set(this);
            }
        }

        public async Task InitializeAsync(INameCultureProvider service)
        {
            var categoryDict = await service.GetAllCategoriesAsync();
            Categories.Clear();
            foreach (var category in categoryDict.Values)
            {
                Categories.Add(category);
            }

            SelectedCategory = Categories.FirstOrDefault();
        }

        private void GenerateName()
        {
            if (_metadata != null)
            {
                var options = new NameGenerationOptions
                {
                    Gender = Gender,
                    SequenceSize = SequenceSize,
                    LengthModifier = LengthModifier
                };
                var name = _metadata.GenerateName(options);
                GeneratedNames.Add(name);
            }
        }

        private CancellationTokenSource? _cts;

        private async Task LoadCultureMetadataAsync(NameCulture? culture)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            if (culture == null)
                return;

            try
            {
                Metadata = await _service.GetCultureMetadataAsync(culture.Category, culture.Name, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Metadata loading canceled.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading metadata: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
