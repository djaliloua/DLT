using ManagPassWord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagPassWord.ViewModels
{
    [QueryProperty(nameof(PassWord), "PassWord")]
    public class DetailsViewModel:BaseViewModel
    {
        public static event Action<PasswordModel> DetailsChanged;
        protected virtual void OnDetailsChanged(PasswordModel detailsViewModel) => DetailsChanged?.Invoke(detailsViewModel);
        private bool _isVisibility = false;
        public bool IsVisibility
        {
            get => _isVisibility;
            set => UpdateObservable(ref _isVisibility, value); 
        }
        private PasswordModel password;
        public PasswordModel PassWord
        {
            get => password; 
            set => UpdateObservable(ref password, value);
        }
        public ICommand CopyCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        
        public DetailsViewModel()
        {
            CopyCommand = new Command(copy);
            UpdateCommand = new Command(update);
        }
        private async void update(object o)
        {
            string _password = await Shell.Current.DisplayPromptAsync("Update", "New password?");
            await Task.Delay(100);
            if (!string.IsNullOrEmpty(_password))
            {
                PassWord.Password = _password;
                OnDetailsChanged(PassWord);
                
                await Shell.Current.DisplayAlert("Update", "Success", "Ok");
            }
            else
                await Shell.Current.DisplayAlert("Update", "Cancelled", "Ok");
        }
        private async void copy(object o)
        {
            await Clipboard.Default.SetTextAsync(PassWord.Password);

        }
    }
    
}
