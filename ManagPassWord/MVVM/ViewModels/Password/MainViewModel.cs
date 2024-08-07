using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.Pages;
using Mapster;
using Patterns.Abstractions;
using Patterns.Implementations;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class LoaddUserService : ILoadService<WebDto>
    {
        public IList<WebDto> Reorder(IList<WebDto> items)
        {
            return items.OrderByDescending(item => item.Id).ToList();
        }
        
    }
    public class LoadableWebDto: Loadable<WebDto>
    {
        public LoadableWebDto(ILoadService<WebDto> loadService):base(loadService)
        {
            
        }
        public override bool ItemExist(WebDto item)
        {
            return Items.FirstOrDefault(w => w.Url == item.Url) != null;
        }
        protected override int Index(WebDto item)
        {
            WebDto w = GetWebElementByUrl(item.Url);
            return base.Index(w);
        }
        public WebDto GetWebElementByUrl(string url)
        {
            return Items.FirstOrDefault(w => w.Url == url);
        }
        
    }
    public class MainViewModel : LoadableWebDto
    {
        private readonly IPasswordRepository _passwordRepository;

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        #endregion

        #region Constructor
        public MainViewModel(IPasswordRepository _db, 
            ILoadService<WebDto> loadService):base(loadService)
        {
            _passwordRepository = _db;
            load();
            CommandSetup();
        }
        #endregion

        #region Private methods
        private async void load()
        {
            ShowActivity();
            var data = await _passwordRepository.GetAllItemsAsync();
            var dataAsDtos = data.Adapt<List<WebDto>>();
            await Task.Run(async() => await LoadItems(dataAsDtos));
            HideActivity();
        }
        private void CommandSetup()
        {
            AddCommand = new Command(OnAdd);
            OpenCommand = new Command(OnOpen);
            SettingCommand = new Command(OnSetting);
            AboutCommand = new Command(OnAbout);
        }
        #endregion

        #region Handlers
        private async void OnOpen(object sender)
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
        private async void OnAdd(object sender)
        {
            SelectedItem = null;
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "password", new PasswordDto() },
                            { "url", IsSelected? SelectedItem.Url: "" },
                            { "isedit", false }
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);

        }
        private async void OnAbout(object sender)
        {
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }
        private async void OnSetting(object sender)
        {
            await Shell.Current.GoToAsync(nameof(SettingPage));
        }
        #endregion
    }
}
