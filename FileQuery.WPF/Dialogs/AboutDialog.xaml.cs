using System.Reflection;
using System.Windows;

namespace FileQuery.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();

            DataContext = new AboutViewModel
            {
                Version = "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString()
            };
        }
    }

    class AboutViewModel
    {
        public string Version { get; set; }
    }
}
