using FileQuery.Wpf.Properties;
using FileQuery.Wpf.Util;

namespace FileQuery.Wpf.ViewModels
{
    public class SearchResultsViewModel : ViewModelBase
    {
        private string _BackgroundColor;
        private string _ForegroundColor;

        public SearchResultsViewModel()
        {
            BackgroundColor = AppSettingsFacade.Instance.Results.BackgroundColor;
            ForegroundColor = AppSettingsFacade.Instance.Results.ForegroundColor;
            // Keep the view model up to date with the settings
            Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ResultsBackground")
            {
                BackgroundColor = AppSettingsFacade.Instance.Results.BackgroundColor;
            }
            else if (e.PropertyName == "ResultsForegound")
            {
                ForegroundColor = AppSettingsFacade.Instance.Results.ForegroundColor;
            }
        }

        public string BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }

            set
            {
                _BackgroundColor = value;
                NotifyPropertyChanged();
            }
        }

        public string ForegroundColor
        {
            get
            {
                return _ForegroundColor;
            }

            set
            {
                _ForegroundColor = value;
                NotifyPropertyChanged();
            }
        }
    }
}
