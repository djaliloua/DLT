using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public class MainPageViewModel : BaseViewModel
    {
        //private readonly DatabaseContext _db;
        private readonly IRepository<User> repository;
        public ObservableCollection<User> Users { get; private set; }
        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set => UpdateObservable(ref _selectedUser, value);
        }
        private bool CanOpen => SelectedUser != null;
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public MainPageViewModel(IRepository<User> _db)
        {
            repository = _db;
            load();
            AddCommand = new Command(On_Add);
            OpenCommand = new Command(On_Open);
            SettingCommand = new Command(On_Setting);
            AboutCommand = new Command(On_About);
        }
        private async void On_About(object sender)
        {
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }
        private async void On_Setting(object sender)
        {
            await Shell.Current.GoToAsync(nameof(SettingPage));
        }
        private async void load()
        {
            await Load();
        }
        public async Task<int> Load()
        {
            var repo = await repository.GetAll();
            Users ??= new ObservableCollection<User>();
            Users.Clear();
            foreach (User item in repo)
            {
                Users.Add(item);
            }
            OnPropertyChanged(nameof(Users));
            return 0;
        }
        private async void On_Open(object sender)
        {
            if (CanOpen)
            {
                var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", SelectedUser },
                            { "isedit", false },
                        };
                await Shell.Current.GoToAsync(nameof(DetailPage), navigationParameter);
            }
        }
        private async void On_Add(object sender)
        {
            SelectedUser = null;
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", new User() },
                            { "isedit", false }
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);
           
        }
    }
}
