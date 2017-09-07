namespace FileQuery.Wpf.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        private bool _UseCustomViewer;
        private string _CustomViewerPath;

        public bool UseNotepad { get; set; }

        public bool UseAssociatedViewer { get; set; }

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
    }
}
