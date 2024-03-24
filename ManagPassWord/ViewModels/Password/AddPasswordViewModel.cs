using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public class AddPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IRepository<User> _db;
        private string _site = "";
        public string Site
        {
            get => _site;
            set => UpdateObservable(ref _site, value);
        }
        private string _userName = "";
        public string UserName
        {
            get => _userName;
            set => UpdateObservable(ref _userName, value);
        }
        private string _password = "";
        public string Password
        {
            get => _password;
            set => UpdateObservable(ref _password, value);
        }
        private string _note = "";
        public string Note
        {
            get => _note;
            set => UpdateObservable(ref _note, value);
        }
        private User _current;
        private bool _isEditPage;
        public bool IsEditPage
        {
            get => _isEditPage;
            set => UpdateObservable(ref _isEditPage, value);
        }
        private bool CanOpen => _current != null;
        public ICommand SaveCommand { get; private set; }
        public AddPasswordViewModel(IRepository<User> db)
        {
            _db = db;
            SaveCommand = new Command(On_Save);
        }
        private async void On_Save(object sender)
        {
            if (!IsEditPage)
            {
                if (!string.IsNullOrEmpty(Site) && !string.IsNullOrEmpty(Password))
                {
                    await _db.SaveItemAsync(new(Site, UserName, Password, Note));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Site or Password field is or are empty.", "OK");
                    return;
                }
            }
            else
            {
                update();
                await _db.SaveItemAsync(_current);
            }
            clearFields();
            await Shell.Current.GoToAsync("..");
            //ServiceLocator.RunMethod<MainPageViewModel>("Load");
            MainPageViewModel mainPageViewModel = ViewModelServices.GetMainPageViewModel();
            await mainPageViewModel.Load();
        }
        private void clearFields()
        {
            Note = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Site = string.Empty;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                _current = query["user"] as User;
                IsEditPage = (bool)query["isedit"];
                setFields();
            }
        }
        private void setFields()
        {
            if (IsEditPage && CanOpen)
            {
                Note = _current.Note;
                UserName = _current.Username ?? string.Empty;
                Password = _current.Password;
                Site = _current.Site;
            }
        }
        private void update()
        {
            _current.Note = Note;
            _current.Username = UserName;
            _current.Password = Password;
            _current.Site = Site;
        }

    }
}
