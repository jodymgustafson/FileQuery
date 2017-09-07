using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileQuery.Wpf.ViewModels
{
    // Base class for all objects that implement INotifyPropertyChanged
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Automatically figures out the property name and fires PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
