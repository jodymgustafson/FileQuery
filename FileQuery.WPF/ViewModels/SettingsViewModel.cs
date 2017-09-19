namespace FileQuery.Wpf.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        private bool _UseCustomViewer;
        private string _CustomViewerPath;

        public bool UseNotepad { get; set; }

        public bool UseAssociatedViewer { get; set; }

        private string _ResultsBackground;
        private string _ResultsForeground;

        public bool UseCustomViewer
        {
            get { return _UseCustomViewer; }
            set
            {
                _UseCustomViewer = value;
                NotifyPropertyChanged();
            }
        }

        public string CustomViewerPath
        {
            get { return _CustomViewerPath; }
            set
            {
                _CustomViewerPath = value;
                NotifyPropertyChanged();
            }
        }

        public string ResultsBackground
        {
            get
            {
                return _ResultsBackground;
            }

            set
            {
                _ResultsBackground = value;
                NotifyPropertyChanged();
            }
        }

        public string ResultsForeground
        {
            get
            {
                return _ResultsForeground;
            }

            set
            {
                _ResultsForeground = value;
                NotifyPropertyChanged();
            }
        }
    }
}
