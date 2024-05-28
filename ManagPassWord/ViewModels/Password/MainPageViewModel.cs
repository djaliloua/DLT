using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public abstract class LoadableMainPageViewModel<TItem> : Loadable<TItem> where TItem : User
    {
        public override void Reorder()
        {
            var data = GetItems().OrderByDescending(item => item.Id).ToList();  
            SetItems(data);
        }
        public override void Update(TItem item)
        {
            int index = GetItems().IndexOf(item);
            DeleteItem(item);
            Items.Insert(index, item);
        }
    }
    public class MainPageViewModel : LoadableMainPageViewModel<User>
    {
        private readonly IRepository<User> repository;
        
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
            await LoadItems();
        }
        public override async Task LoadItems()
        {
            var repo = await repository.GetAll();
            SetItems(repo);
        }
        
        private async void On_Open(object sender)
        {
            if (IsSelected)
            {
                var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", SelectedItem },
                            { "isedit", false },
                        };
                await Shell.Current.GoToAsync(nameof(DetailPage), navigationParameter);
            }
        }
        private async void On_Add(object sender)
        {
            SelectedItem = null;
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", new User() },
                            { "isedit", false }
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);
           
        }
        
    }
}
