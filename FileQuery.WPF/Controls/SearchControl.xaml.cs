using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileQuery.Core;
using FileQuery.Wpf.Dialogs;
using FileQuery.Wpf.Util;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        private SearchControlViewModel ViewModel
        {
            get { return DataContext as SearchControlViewModel; }
        }

        private QueryProcessManager _queryProc;
        private QueryProcessManager QueryProc
        {
            get
            {
                return _queryProc ?? (_queryProc = new QueryProcessManager(ViewModel, Dispatcher));
            }
        }

        public SearchControl()
        {
            InitializeComponent();

            // Init the query to the one stored in app settings
            SearchControlViewModel query = null;
            try
            {
                query = AppSettingsFacade.Instance.SearchQuery;
            }
            catch (Exception ex)
            {
                AppSettingsFacade.Instance.SearchQuery = null;
                MessageBox.Show("Error loading search query from app settings: " + ex.Message);
            }
            DataContext = query ?? new SearchControlViewModel();
        }

        private void ValidateSearchQuery(Query query)
        {
            if (query.FileSources.Count() == 0 || ViewModel.SearchParams.Count == 0)
            {
                MessageBox.Show("You must define at least one search path and one search parameter", "Missing Parameters", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsDialog().ShowDialog();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void NewCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            ViewModel.SearchParams.Clear();
            ViewModel.SearchPaths.Clear();
            ViewModel.SearchResults.Clear();
        }

        private void NewCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.IsNotSearching;
        }

        private void SaveCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, string.Join("\n", ViewModel.SearchResults));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExitCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ViewModel.IsSearching)
            {
                MessageBox.Show("A search is currently running. Stop the search before closing the application.", "File Query", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            App.Current.Shutdown();
        }

        private void ExecuteCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Toggles the search state: If running it stops, otherwise it starts
            if (QueryProc.IsRunning)
            {
                QueryProc.Abort();
            }
            else
            {
                try
                {
                    Query query = SearchQueryFactory.GetSearchQuery(ViewModel);
                    ValidateSearchQuery(query);
                    ViewModel.SearchResults.Clear();
                    // Save the view model to app settings
                    AppSettingsFacade.Instance.SearchQuery = ViewModel;
                    QueryProc.Start(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Creating Query", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveQueryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "yml";
            dlg.Filter = "yml files (*.yml)|*.yml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var yaml = SearchQuerySerializer.ToYaml(ViewModel);
                    System.IO.File.WriteAllText(dlg.FileName, yaml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenQueryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = "yml files (*.yml)|*.yml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var reader = new StreamReader(dlg.OpenFile()))
                    {
                        DataContext = SearchQuerySerializer.FromYaml(reader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening query: " + ex.Message, "Open Query Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
