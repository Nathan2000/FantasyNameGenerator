using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyNameGenerator.Lib.Domain.Common;
using FantasyNameGenerator.Lib.Domain.Model;
using FantasyNameGenerator.Lib.Domain.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace FantasyNameGenerator.WPF.ViewModels.Main
{
    public partial class MainViewModel : ObservableObject, IMainViewModel, IDisposable
    {
        private readonly INameCultureProvider _cultureProvider;

        public ObservableCollection<NameCategory> Categories { get; } = [];
        public ObservableCollection<NameCulture> Cultures { get; } = [];

        private NameCategory? _selectedCategory;
        public NameCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (!SetProperty(ref _selectedCategory, value))
                    return;
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
                if (!SetProperty(ref _selectedCulture, value))
                    return;
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
                SetProperty(ref _metadata, value);
                GenerateNameCommand.NotifyCanExecuteChanged();
            }
        }

        public Gender Gender { get; set; }
        public int SequenceSize { get; set; } = 3;
        public double LengthModifier { get; set; } = 1.0;

        private CancellationTokenSource? _cts;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GenerateNameCommand))]
        private bool _isLoading;

        public MainViewModel(INameCultureProvider cultureProvider)
        {
            _cultureProvider = cultureProvider;
            GeneratedNames = [];

            _ = InitializeAsync();
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                var categoryDict = await _cultureProvider.GetAllCategoriesAsync();
                Categories.Clear();
                foreach (var category in categoryDict.Values)
                {
                    Categories.Add(category);
                }

                SelectedCategory = Categories.FirstOrDefault();

            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanGenerateName))]
        private async Task GenerateNameAsync()
        {
            var metadata = Metadata;
            if (metadata == null)
                return;

            var options = new NameGenerationOptions
            {
                Gender = Gender,
                SequenceSize = SequenceSize,
                LengthModifier = LengthModifier
            };

            var name = await Task.Run(() => metadata.GenerateName(options)).ConfigureAwait(false);

            if (Application.Current?.Dispatcher != null)
                await Application.Current.Dispatcher.InvokeAsync(() => GeneratedNames.Add(name));
            else
                GeneratedNames.Add(name);

        }

        private bool CanGenerateName()
        {
            return Metadata != null;
        }

        [RelayCommand(CanExecute = nameof(CanClearNames))]
        private void ClearNames()
        {
            GeneratedNames.Clear();
        }

        private bool CanClearNames()
        {
            return GeneratedNames != null && GeneratedNames.Any();
        }

        private async Task LoadCultureMetadataAsync(NameCulture? culture)
        {
            // If previous request hasn't finished, cancel it.
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            if (culture == null)
                return;

            IsLoading = true;
            try
            {
                var fetched = await _cultureProvider.GetCultureMetadataAsync(culture.Category, culture.Name, token);
                if (token.IsCancellationRequested)
                    return;

                Metadata = fetched;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Metadata loading canceled.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading metadata: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
