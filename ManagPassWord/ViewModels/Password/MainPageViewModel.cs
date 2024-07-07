using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using Mapster;
using Patterns;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public abstract class LoadableMainPageViewModel<TItem> : Loadable<TItem> where TItem : UserDTO
    {
        protected override void Reorder()
        {
            var data = GetItems().OrderByDescending(item => item.Id).ToList();  
            SetItems(data);
        }
       
    }
    public class MainPageViewModel : LoadableMainPageViewModel<UserDTO>
    {
        private readonly IPasswordRepository repository;
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public MainPageViewModel(IPasswordRepository _db)
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
            var repo = await repository.GetAllItemsAsync();
            var data = repo.Adapt<List<UserDTO>>();
            SetItems(data);
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
                            { "user", new UserDTO() },
                            { "isedit", false }
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);
           
        }
        
    }
}
