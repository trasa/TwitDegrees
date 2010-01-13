using System.ComponentModel;
using System.Windows;

namespace TwitDegrees.GraphViewer.Infrastructure
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            WireUpEvents();
        }

        protected virtual void WireUpEvents()
        {
            // no events here.
        }

        protected static Visibility GetPanelVisibleState(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        protected void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null) h(this, e);
        }


        public event PropertyChangedEventHandler PropertyChanged;

    }
}
