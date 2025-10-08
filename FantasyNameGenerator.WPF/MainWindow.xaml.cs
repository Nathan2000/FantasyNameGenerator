﻿using FantasyNameGenerator.WPF.Services;
using FantasyNameGenerator.WPF.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FantasyNameGenerator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (DataContext is NamesViewModel vm)
            {
                await vm.InitializeAsync(ServiceLocator.NameCultureProvider);
            }
        }

        private void CommandBinding_OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandBinding_OnCanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            var listbox = (ListBox)sender;
            e.CanExecute = listbox.SelectedItem != null && listbox.SelectedItem is string;
        }

        private void CommandBinding_OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            var listbox = (ListBox)sender;
            if (listbox.SelectedItem is string selectedName)
            {
                Clipboard.SetText(selectedName);
            }
        }

        private void CommandBinding_OnCanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataContext is NamesViewModel viewModel)
            {
                e.CanExecute = viewModel.GeneratedNames.Any();
            }
        }

        private async void CommandBinding_OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is NamesViewModel viewModel)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FileName = "names",
                    DefaultExt = ".txt",
                    AddExtension = true
                };
                var success = dialog.ShowDialog(this);
                if (success == true)
                {
                    try
                    {
                        using var file = new StreamWriter(dialog.OpenFile());
                        foreach (var name in viewModel.GeneratedNames)
                        {
                            await file.WriteLineAsync(name.FullName);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}