using System;
using System.Windows;
using System.Windows.Controls;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Controls
{
    /// <summary>
    /// Interaction logic for SearchPathItem.xaml
    /// </summary>
    public partial class SearchPathItem : UserControl
    {
        private SearchPathItemViewModel ViewModel
        {
            get { return DataContext as SearchPathItemViewModel; }
        }

        public SearchPathItem()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsResultsFile)
            {
                SelectFile();
            }
            else
            {
                SelectFolder();
            }
        }

        private void SelectFile()
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.DefaultExt = "txt";
            dlg.CheckFileExists = true;
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel.PathValue = dlg.FileName;
            }
        }

        private void SelectFolder()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.SelectedPath = ViewModel.PathValue;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ViewModel.PathValue = dlg.SelectedPath;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // This will cause the item to be deleted
            ViewModel.RemoveItem = true;
        }
    }
}
