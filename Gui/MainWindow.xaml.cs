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
    using System.IO;

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

        private void CommandBinding_OnCanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            // TODO: This method isn't run when items are generated through the Generate button. Do something about it.
            var listBox = (ListBox)sender;
            e.CanExecute = listBox.HasItems;
        }

        private void CommandBinding_OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            var listBox = (ListBox)sender;
            SaveFileDialog dialog = new SaveFileDialog
                                        {
                                            Filter = "Text files|*.txt|All types|*.*",
                                            AddExtension = true,
                                            FileName = "Generated names",
                                            DefaultExt = "txt",
                                        };
            var success = dialog.ShowDialog();
            if (success == true)
            {
                try
                {
                    using (var file = new StreamWriter(dialog.OpenFile()))
                    {
                        foreach (var item in listBox.Items)
                        {
                            var name = item as NameData;
                            if (name == null)
                            {
                                continue;
                            }

                            file.WriteLine(name.Name);
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error when writing to the " + dialog.FileName + " file.\r\n\r\n" + ex.Message);
                }
            }
        }
    }
}
