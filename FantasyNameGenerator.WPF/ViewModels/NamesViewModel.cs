using FantasyNameGenerator.Lib.Composer;
using FantasyNameGenerator.Lib.Metadata;
using FantasyNameGenerator.WPF.Commands;
using FantasyNameGenerator.WPF.Model;
using FantasyNameGenerator.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FantasyNameGenerator.WPF.ViewModels
{
    internal class NamesViewModel : INotifyPropertyChanged
    {
        private NameComposer? _composer;

        public ObservableCollection<NameCategory> Categories { get; set; }
        public ObservableCollection<string> Names { get; set; }

        private NameCategory? _selectedCategory;
        public NameCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                OnPropertyChanged();
                SelectedCulture = _selectedCategory?.Cultures.FirstOrDefault();
            }
        }

        private NameCulture? _selectedCulture;
        public NameCulture? SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                if (_selectedCulture == value) return;
                _selectedCulture = value;
                OnPropertyChanged();
                ResetComposer();
            }
        }

        private Gender _gender = Gender.Male;
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (_gender == value) return;
                _gender = value;
                ResetComposer();
            }
        }

        private int _sequenceSize = 3;
        public int SequenceSize
        {
            get => _sequenceSize;
            set
            {
                if (_sequenceSize == value) return;
                _sequenceSize = value;
                ResetComposer();
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ICommand ClearNamesCommand { get; set; }

        public NamesViewModel() : this(ServiceLocator.DataService) { }
        public NamesViewModel(IDataService dataService)
        {
            Categories = dataService.GetCategories();
            SelectedCategory = Categories.FirstOrDefault();
            Names = [];

            GenerateCommand = new SimpleCommand(
                p => GenerateNames(),
                p => SelectedCategory != null && SelectedCulture != null);
            ClearNamesCommand = new SimpleCommand(
                p => Names.Clear(),
                p => Names.Any());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetComposer()
        {
            if (SelectedCulture == null || SelectedCulture.Metadata == null || SelectedCulture.DirectoryPath == null)
            {
                _composer = null;
                return;
            }

            var metadata = JsonMetadataMapper.Map(SelectedCulture.Name, SelectedCulture.DirectoryPath, SelectedCulture.Metadata);
            _composer = new NameComposer(Gender, SequenceSize, metadata);
        }

        private void GenerateNames()
        {
            if (_composer != null)
            {
                var name = _composer.Generate();
                Names.Add(name);
            }
        }
    }
}
