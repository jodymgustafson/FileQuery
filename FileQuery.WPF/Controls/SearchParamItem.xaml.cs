using System.Windows;
using System.Windows.Controls;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Controls
{
    /// <summary>
    /// Interaction logic for SearchParamItem.xaml
    /// </summary>
    public partial class SearchParamItem : UserControl
    {
        private SearchParamItemViewModel ViewModel
        {
            get { return DataContext as SearchParamItemViewModel; }
        }

        public SearchParamItem()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveItem = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
