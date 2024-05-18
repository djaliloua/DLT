using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected virtual void OnShow()
        {

        }
        public BaseViewModel()
        {
            Show = false;
        }
        //public abstract Task Load();
        private bool show;
        public bool Show
        {
            get => show; 
            set => UpdateObservable(ref show, value, () => OnShow());
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public event EventHandler OnLoad;
        public void ShowProgressBar()
        {
            Show = true;
        }
        public void HideProgressBar()
        {
            Show = false;
        }
        public void UpdateObservable<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            OnPropertyChanged(propertyName);
        }
        public void UpdateObservable<T>(ref T oldValue, T newValue, Action callback, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            OnPropertyChanged(propertyName);
            callback();
        }
    }
}
