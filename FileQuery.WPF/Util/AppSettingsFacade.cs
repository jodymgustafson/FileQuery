using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileQuery.Wpf.Properties;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Util
{
    enum FileViewerType
    {
        Notepad,
        Custom,
        Associated
    }

    class AppSettingsFacade
    {
        private static AppSettingsFacade _Instance;
        private static ResultsSettings _results;

        public static AppSettingsFacade Instance
        {
            get { return _Instance ?? (_Instance = new AppSettingsFacade()); }
        }

        public ResultsSettings Results
        {
            get
            {
                if (_results == null)
                {
                    _results = new ResultsSettings();
                }
                return _results;
            }

            set
            {
                _results = value;
                Settings.Default.ResultsBackground = value.BackgroundColor;
                Settings.Default.ResultsForegound = value.ForegroundColor;
            }
        }

        public FileViewerType FileViewerType
        {
            get
            {
                var fileViewerApp = Settings.Default.FileViewerApp;
                switch (fileViewerApp)
                {
                    case "notepad.exe":
                        return FileViewerType.Notepad;
                    case "__associated__":
                        return FileViewerType.Associated;
                    default:
                        return FileViewerType.Custom;
                }
            }

            set
            {
                switch (value)
                {
                    case FileViewerType.Notepad:
                        Settings.Default.FileViewerApp = "notepad.exe";
                        break;
                    case FileViewerType.Associated:
                        Settings.Default.FileViewerApp = "__associated__";
                        break;
                    case FileViewerType.Custom:
                        Settings.Default.FileViewerApp = "";
                        break;
                }
            }
        }

        public string FileViewPath
        {
            get
            {
                if (FileViewerType != FileViewerType.Associated)
                {
                    return Settings.Default.FileViewerApp;
                }
                else return "";
            }

            set
            {
                Settings.Default.FileViewerApp = value;
            }
        }

        public SearchControlViewModel SearchQuery
        {
            get
            {
                var yaml = Settings.Default.SearchQuery;
                if (!string.IsNullOrEmpty(yaml))
                {
                    return SearchQuerySerializer.FromYaml(yaml);
                }
                return null;
            }
            set
            {
                Settings.Default.SearchQuery = value == null ? null : SearchQuerySerializer.ToYaml(value);
            }
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }

    class ResultsSettings
    {
        public string BackgroundColor
        {
            get
            {
                return Settings.Default.ResultsBackground;
            }
            set
            {
                Settings.Default.ResultsBackground = value;
            }
        }

        public string ForegroundColor
        {
            get
            {
                return Settings.Default.ResultsForegound;
            }
            set
            {
                Settings.Default.ResultsForegound = value;
            }
        }

        public const string DefaultBackground = "#111111";
        public const string DefaultForeground = "#FF32CD32";
    }
}
