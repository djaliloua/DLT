using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp.ViewModels
{
    public interface IActivity
    {
        bool IsActivity { get; set; }
        void ShowActivity();
        void HideActivity();
    }
    public class BaseViewModel : INotifyPropertyChanged, IActivity
    {

        protected virtual void OnShow()
        {

        }
        public BaseViewModel()
        {
            IsActivity = false;
        }

        private bool _isActivity;
        public bool IsActivity
        {
            get => _isActivity;
            set => UpdateObservable(ref _isActivity, value);
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

        public void ShowActivity()
        {
            IsActivity = true;
            OnShow();
        }

        public void HideActivity()
        {
            IsActivity = false;
            OnShow();
        }
    }
}
