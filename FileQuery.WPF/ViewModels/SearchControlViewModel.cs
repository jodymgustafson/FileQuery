using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FileQuery.Wpf.ViewModels
{
    public class SearchControlViewModel : ViewModelBase
    {
        private ObservableCollection<SearchPathItemViewModel> _SearchPaths = new ObservableCollection<SearchPathItemViewModel>();
        private ObservableCollection<SearchParamItemViewModel> _SearchParams = new ObservableCollection<SearchParamItemViewModel>();
        private ObservableCollection<string> _SearchResults = new ObservableCollection<string>();
        private bool _IsSearching = false;

        public SearchControlViewModel()
        {
            SearchResults.CollectionChanged += SearchResults_CollectionChanged;
            SearchPaths.CollectionChanged += SearchPaths_CollectionChanged;
            SearchParams.CollectionChanged += SearchParams_CollectionChanged;
            ResultsSettings = new SearchResultsViewModel();
        }

        public SearchResultsViewModel ResultsSettings { get; set; }

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
        }

        public ObservableCollection<string> SearchResults
        {
            get
            {
                return _SearchResults;
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

        #region Collection changed listeners

        private void SearchParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // When items are added...
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SearchParamItemViewModel item in e.NewItems)
                {
                    // We need to listen for when an item is marked to be removed
                    item.PropertyChanged += SearchParamItem_PropertyChanged; ;
                }
            }
        }

        private void SearchParamItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemoveItem")
            {
                var item = sender as SearchParamItemViewModel;
                SearchParams.Remove(item);
                item.PropertyChanged -= SearchParamItem_PropertyChanged;
            }
        }

        private void SearchPaths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // When items are added...
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SearchPathItemViewModel item in e.NewItems)
                {
                    // We need to listen for when an item is marked to be removed
                    item.PropertyChanged += SearchPathItem_PropertyChanged;
                }
            }
        }

        private void SearchPathItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemoveItem")
            {
                var item = sender as SearchPathItemViewModel;
                SearchPaths.Remove(item);
                item.PropertyChanged -= SearchPathItem_PropertyChanged;
            }
        }

        private void SearchResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FileCount");
        }

        #endregion
    }
}
