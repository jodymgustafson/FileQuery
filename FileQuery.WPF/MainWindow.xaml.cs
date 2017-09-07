using System.Windows;
using FileQuery.Wpf.Properties;

namespace FileQuery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckVersionUpgrade();
        }

        /// <summary>
        /// This will upgrade the user's settings from the previous version of the app
        /// </summary>
        private void CheckVersionUpgrade()
        {
            if (Settings.Default.UpgradeSettings)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeSettings = false;
            }
        }
    }
}
