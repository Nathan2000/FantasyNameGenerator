using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui
{
    using Gui.Model;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            var listbox = (ListBox)sender;
            var selectedName = (NameData)listbox.SelectedItem;
            Clipboard.SetText(selectedName.Name);
        }

        private void CommandBinding_OnCanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            var listbox = (ListBox)sender;
            e.CanExecute = listbox.SelectedItem != null;
        }

        private void CommandBinding_OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            var window = (Window)sender;
            window.Close();
        }
    }
}
