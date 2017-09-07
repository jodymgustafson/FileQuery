using System;
using System.Collections.Specialized;
using System.ComponentModel;
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
        private BackgroundWorker backgroundWorker;
        private FileQueryProcessor queryProc;

        private SearchControlViewModel ViewModel
        {
            get { return DataContext as SearchControlViewModel; }
        }

        public SearchControl()
        {
            InitializeComponent();

            var query = AppSettingsFacade.Instance.SearchQuery;
            DataContext = query ?? new SearchControlViewModel();

            backgroundWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatch(new Action(() => ViewModel.IsSearching = false));
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatch(new Action(() => ViewModel.IsSearching = true));
            RunFileQuery(e.Argument as Query);
        }

        private void RunFileQuery(Query query)
        {
            if (queryProc == null)
            {
                queryProc = new FileQueryProcessor();
                queryProc.FileFound += QueryProc_FileFound;
            }

            try
            {
                queryProc.Execute(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                queryProc.Cancel();
                backgroundWorker.CancelAsync();
            }
            else
            {
                try
                {
                    Query query = SearchQueryFactory.GetSearchQuery(ViewModel);
                    ValidateSearchQuery(query);
                    ViewModel.SearchResults.Clear();
                    AppSettingsFacade.Instance.SearchQuery = ViewModel;
                    AppSettingsFacade.Instance.Save();
                    backgroundWorker.RunWorkerAsync(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Creating Query", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ValidateSearchQuery(Query query)
        {
            if (query.FileSources.Count() == 0 || ViewModel.SearchParams.Count == 0)
            {
                MessageBox.Show("You must define at least one search path and one search parameter", "Missing Parameters", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void QueryProc_FileFound(object sender, FileFoundEventArgs e)
        {
            Dispatch(new Action(() => ViewModel.SearchResults.Add(e.fileInfo.FullName)));
        }

        private void Dispatch(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        private void Results_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    ViewModel.SearchResults.Clear();
                    break;
                default:
                    foreach (string i in e.NewItems)
                    {
                        ViewModel.SearchResults.Add(i);
                    }
                    break;
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingsDialog().ShowDialog();
        }

        private void SaveResults()
        {
            var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(dlg.FileName, string.Join("\n", ViewModel.SearchResults));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
            SaveResults();
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
            // This will cause any text fields to lose focus and update the view model
            StartSearch.Focus();
            SearchButton_Click(sender, e);
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
