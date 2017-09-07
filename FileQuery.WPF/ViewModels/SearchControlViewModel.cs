using System.Collections.ObjectModel;

namespace FileQuery.Wpf.ViewModels
{
    class SearchControlViewModel : ViewModelBase
    {
        private ObservableCollection<SearchPathItemViewModel> _SearchPaths = new ObservableCollection<SearchPathItemViewModel>();
        private ObservableCollection<SearchParamItemViewModel> _SearchParams = new ObservableCollection<SearchParamItemViewModel>();
        private ObservableCollection<string> _SearchResults = new ObservableCollection<string>();
        private bool _IsSearching = false;

        public SearchControlViewModel()
        {
            SearchResults.CollectionChanged += SearchResults_CollectionChanged;
        }

        private void SearchResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FileCount");
        }

        public ObservableCollection<SearchPathItemViewModel> SearchPaths
        {
            get
            {
                return _SearchPaths;
            }
            set
            {
                _SearchPaths = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<SearchParamItemViewModel> SearchParams
        {
            get
            {
                return _SearchParams;
            }

            set
            {
                _SearchParams = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> SearchResults
        {
            get
            {
                return _SearchResults;
            }

            set
            {
                _SearchResults = value;
                NotifyPropertyChanged();
            }
        }

        public int FileCount
        {
            get
            {
                return SearchResults.Count;
            }
        }

        public bool IsSearching
        {
            get
            {
                return _IsSearching;
            }

            set
            {
                _IsSearching = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsNotSearching");
                NotifyPropertyChanged("SearchStatusText");
            }
        }

        public bool IsNotSearching
        {
            get { return !IsSearching; }
        }

        public string SearchStatusText
        {
            get { return (IsSearching ? "Searching..." : "Ready"); }
        }
    }
}
