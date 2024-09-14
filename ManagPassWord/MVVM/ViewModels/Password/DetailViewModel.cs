using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.DataAcessLayer.Implementations;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using Patterns.Abstractions;
using Patterns.Implementations;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class LoadPasswordService : ILoadService<PasswordDto>
    {
        public IList<PasswordDto> Reorder(IList<PasswordDto> items)
        {
            return items.OrderByDescending(x => x.Id).ToList();
        }
    }
    public class DetailViewModel : Loadable<PasswordDto>, IQueryAttributable
    {
        private WebDto _userDetail;
        public WebDto WebSitePasswords
        {
            get => _userDetail;
            set => UpdateObservable(ref _userDetail, value, async() =>
            {
                if (WebSitePasswords != null)
                {
                    ShowActivity();
                    await Task.Run(async () => await LoadItems(WebSitePasswords.Passwords));
                    HideActivity();
                }
            });
        }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public DetailViewModel(ILoadService<PasswordDto> loadService):base(loadService)
        {
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
        }
        
        private async void OnDelete(object sender)
        {
            PasswordDto tempPassword = sender as PasswordDto;
            SelectedItem = tempPassword;
            if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
            {
                if (tempPassword.Id != 0)
                {
                    using PasswordRepository passwordRepository = new PasswordRepository();
                    if (await passwordRepository.GetItemByUrl(tempPassword?.Web.Url) is Web web)
                    {
                        web.DeletePasswordItem(tempPassword);
                        await passwordRepository.SaveOrUpdateItemAsync(web);
                        DeleteItem(SelectedItem);
                        ViewModelLocator.MainViewModel.UpdateItem(web.Adapt<WebDto>());
                    }
                }
            }
        }
        private async void OnEdit(object sender)
        {
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "password", sender },
                            { "isedit", true },
                            { "url", WebSitePasswords.Url },
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                WebSitePasswords = query["user"] as WebDto;
            }
        }
    }
}
