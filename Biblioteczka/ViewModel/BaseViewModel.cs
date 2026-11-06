using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Biblioteczka.UI.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public abstract string Title { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
