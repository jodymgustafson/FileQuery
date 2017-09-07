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
                // Start with one path and param
                AddSearchPath();
                AddSearchParam();
            }
        }

        private void AddSearchPath()
        {
            var item = new SearchPathItemViewModel();
            item.PropertyChanged += PathItem_PropertyChanged;
            ViewModel.SearchPaths.Add(item);
        }

        private void AddSearchParam()
        {
            var item = new SearchParamItemViewModel();
            item.PropertyChanged += ParamItem_PropertyChanged;
            ViewModel.SearchParams.Add(item);
        }

        private void AddPath_Click(object sender, RoutedEventArgs e)
        {
            AddSearchPath();

            //if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    item.PathValue = folderDlg.SelectedPath;
            //}
        }

        private void PathItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemoveItem")
            {
                var item = (sender as SearchPathItemViewModel);
                item.PropertyChanged -= PathItem_PropertyChanged;
                ViewModel.SearchPaths.Remove(item);
            }
        }

        private void ResetPaths_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchPaths.Clear();
        }

        private void AddParam_Click(object sender, RoutedEventArgs e)
        {
            AddSearchParam();
        }

        private void ParamItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemoveItem")
            {
                var item = (sender as SearchParamItemViewModel);
                item.PropertyChanged -= PathItem_PropertyChanged;
                ViewModel.SearchParams.Remove(item);
            }
        }

        private void ResetParams_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchParams.Clear();
        }
    }
}
