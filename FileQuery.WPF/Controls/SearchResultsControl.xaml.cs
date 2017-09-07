using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileQuery.Wpf.Util;

namespace FileQuery.Controls
{
    /// <summary>
    /// Interaction logic for SearchResultsControl.xaml
    /// </summary>
    public partial class SearchResultsControl : UserControl
    {
        public SearchResultsControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewSelectedFile();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewSelectedFile();
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResultListView.SelectedValue != null;
        }

        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = string.Join("\n", ResultListView.SelectedItems.Cast<string>());
            Clipboard.SetText(txt);
        }

        private void SelectAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResultListView.SelectAll();
        }

        private void ViewSelectedFile()
        {
            var file = ResultListView.SelectedValue as string;
            if (!string.IsNullOrEmpty(file))
            {
                ViewFile(file);
            }
        }

        private void ViewFile(string file)
        {
            try
            {
                var settings = AppSettingsFacade.Instance;
                switch (settings.FileViewerType)
                {
                    case FileViewerType.Custom:
                    case FileViewerType.Notepad:
                        Process.Start(settings.FileViewPath, $"\"{file}\"");
                        break;
                    case FileViewerType.Associated:
                        if (IsOkToOpenFile(file))
                        {
                            // Let system determine appropriate app to open file
                            Process.Start(file);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if a file is executable
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True if the file can be opened</returns>
        private bool IsOkToOpenFile(string file)
        {
            if (file.EndsWith(".exe") || file.EndsWith(".bat"))
            {
                var result = MessageBox.Show("Opening an executable file can be dangerous. Are you sure you want to open it?",
                    "Open File", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                return (result == MessageBoxResult.Yes);
            }

            return true;
        }
    }
}
