using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using Pass=ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class AddPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        #region Private Properties
        private readonly IPasswordRepository _passwordRepository;
        #endregion

        #region Public Properties
        private string _url = "google.com";
        public string Url
        {
            get => _url.ToLower().Trim();
            set => UpdateObservable(ref _url, value);
        }
        private PasswordDto _password = new PasswordDto();
        public PasswordDto Password
        {
            get => _password;
            set => UpdateObservable(ref _password, value);
        }
        private WebDto _current;
        private bool _isEditPage;
        public bool IsEditPage
        {
            get => _isEditPage;
            set => UpdateObservable(ref _isEditPage, value);
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public AddPasswordViewModel(IPasswordRepository db)
        {
            _passwordRepository = db;
            CommandSetup();
        }
        #endregion

        #region Private methods
        private void CommandSetup()
        {
            SaveCommand = new Command(OnSave);
            BackCommand = new Command(OnBack);
        }
        #endregion

        #region Handlers
        private async void OnBack(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave(object sender)
        {
            Web temp_item;
            Password.CreateUpdateDate();
            try
            {
                if (!IsEditPage)
                {
                    if (await _passwordRepository.GetItemByUrl(Url) is Web web)
                    {
                        web.Add(Password.Adapt<Pass.Password>());
                        temp_item = await _passwordRepository.SaveOrUpdateItemAsync(web);
                        ViewModelLocator.MainPageViewModel.UpdateItem(temp_item.Adapt<WebDto>());
                    }
                    else
                    {
                        web = new Web(Url);
                        web.Add(Password.Adapt<Pass.Password>());
                        temp_item = await _passwordRepository.SaveOrUpdateItemAsync(web);
                        ViewModelLocator.MainPageViewModel.AddItem(temp_item.Adapt<WebDto>());
                        await Shell.Current.DisplayAlert("Error", "Site or Password field is or are empty.", "OK");
                        return;
                    }
                }
                else
                {
                    if(await _passwordRepository.GetItemByUrl(Url) is Web web)
                    {
                        web.UpdatePasswordItem(Password.Adapt<Pass.Password>());
                        var webdto = await _passwordRepository.SaveOrUpdateItemAsync(web);
                        ViewModelLocator.MainPageViewModel.UpdateItem(webdto.Adapt<WebDto>());
                    }
                }
                await Shell.Current.GoToAsync("..");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
           
        }
        #endregion
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            
            if (query.Count > 0)
            {
                if(query.TryGetValue("password", out var Pas))
                {
                    Password = query["password"] as PasswordDto;
                }
                IsEditPage = (bool)query["isedit"];
            }
        }
    }
}
