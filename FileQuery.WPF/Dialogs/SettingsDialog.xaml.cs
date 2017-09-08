using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FileQuery.Wpf.Properties;
using FileQuery.Wpf.Util;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private SettingsViewModel ViewModel
        {
            get { return DataContext as SettingsViewModel; }
        }

        public SettingsDialog()
        {
            InitializeComponent();
            DataContext = GetViewModel();
        }

        private SettingsViewModel GetViewModel()
        {
            var vm = new SettingsViewModel();

            var settings = AppSettingsFacade.Instance;
            switch (settings.FileViewerType)
            {
                case FileViewerType.Notepad:
                    vm.UseNotepad = true;
                    break;
                case FileViewerType.Associated:
                    vm.UseAssociatedViewer = true;
                    break;
                default:
                    vm.UseCustomViewer = true;
                    vm.CustomViewerPath = settings.FileViewPath;
                    break;
            }

            return vm;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = AppSettingsFacade.Instance;
            if (ViewModel.UseNotepad)
            {
                settings.FileViewerType = FileViewerType.Notepad;
            }
            else if (ViewModel.UseAssociatedViewer)
            {
                if (!VerifyAssociatedViewerRisk()) return;
                settings.FileViewerType = FileViewerType.Associated;
            }
            else if (ViewModel.UseCustomViewer)
            {
                if (!ValidateCustomViewerPath()) return;
                settings.FileViewPath = ViewModel.CustomViewerPath;
            }

            Close();
        }

        private bool ValidateCustomViewerPath()
        {
            if (string.IsNullOrEmpty(ViewModel.CustomViewerPath))
            {
                System.Windows.MessageBox.Show("You must select the path to the application you want to use as a file viewer.", "File Query", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            return true;
        }

        private bool VerifyAssociatedViewerRisk()
        {
            var result = System.Windows.MessageBox.Show("Opening some files (such as executables) with the associated application may be dangerous. Are you sure you want to enable this option?", 
                "File Query", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }

        private void ChooseAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.UseCustomViewer)
            {
                var dlg = new OpenFileDialog();
                dlg.FileName = ViewModel.CustomViewerPath;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ViewModel.CustomViewerPath = dlg.FileName;
                }
            }
        }
    }
}
