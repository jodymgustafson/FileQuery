using System.Windows;
using System.Windows.Controls;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Controls
{
    /// <summary>
    /// Interaction logic for SearchParametersControl.xaml
    /// </summary>
    public partial class SearchParametersControl : UserControl
    {
        private System.Windows.Forms.FolderBrowserDialog folderDlg;

        private SearchControlViewModel ViewModel
        {
            get { return DataContext as SearchControlViewModel; }
        }

        public SearchParametersControl()
        {
            InitializeComponent();

            folderDlg = new System.Windows.Forms.FolderBrowserDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                // Start with one path and one param
                if (ViewModel.SearchPaths.Count == 0)
                {
                    AddSearchPath();
                }
                if (ViewModel.SearchParams.Count == 0)
                {
                    AddSearchParam();
                }
            }
        }

        private void AddSearchPath()
        {
            var item = new SearchPathItemViewModel();
            ViewModel.SearchPaths.Add(item);
        }

        private void AddSearchParam()
        {
            var item = new SearchParamItemViewModel();
            ViewModel.SearchParams.Add(item);
        }

        private void AddPath_Click(object sender, RoutedEventArgs e)
        {
            AddSearchPath();
        }

        private void ResetPaths_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchPaths.Clear();
        }

        private void AddParam_Click(object sender, RoutedEventArgs e)
        {
            AddSearchParam();
        }

        private void ResetParams_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchParams.Clear();
        }
    }
}
