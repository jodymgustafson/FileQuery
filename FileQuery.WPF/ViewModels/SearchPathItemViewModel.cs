﻿namespace FileQuery.Wpf.ViewModels
{
    public class SearchPathItemViewModel : ViewModelBase
    {
        private string _PathType;
        private string _PathValue;
        private bool _RemoveItem;

        public SearchPathItemViewModel()
        {
            PathType = "Include-Recursive";
        }

        /// <summary>
        /// Can be Include-Recursive, Include-NoRecure, Exclude
        /// </summary>
        public string PathType
        {
            get
            {
                return _PathType;
            }

            set
            {
                _PathType = value;
                NotifyPropertyChanged();
            }
        }

        public string PathValue
        {
            get
            {
                return _PathValue;
            }

            set
            {
                _PathValue = value;
                NotifyPropertyChanged();
            }
        }

        public bool RemoveItem
        {
            get
            {
                return _RemoveItem;
            }

            set
            {
                _RemoveItem = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsRecursive
        {
            get { return PathType == "Include-Recursive"; }
        }

        public bool IsInclude
        {
            get { return PathType.StartsWith("Include"); }
        }

        public bool IsExclude
        {
            get { return PathType == "Exclude"; }
        }

        public bool IsResultsFile
        {
            get { return PathType == "File"; }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(PathValue);
            }
        }
    }
}
