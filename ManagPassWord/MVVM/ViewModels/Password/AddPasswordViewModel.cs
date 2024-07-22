using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class AddPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IPasswordRepository _passwordRepository;
        private WebDto _user;
        public WebDto User
        {
            get => _user;
            set => UpdateObservable(ref _user, value);
        }
        private string _url = "google.com";
        public string Url
        {
            get => _url;
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
        private bool CanOpen => _current != null;
        public ICommand SaveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public AddPasswordViewModel(IPasswordRepository db)
        {
            _passwordRepository = db;
            User = new();
            SaveCommand = new Command(OnSave);
            BackCommand = new Command(OnBack);
        }

        private async void OnBack(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave(object sender)
        {
            Web temp_item;
            try
            {
                if (!IsEditPage)
                {
                    if (User.IsValid())
                    {
                        temp_item = await _passwordRepository.SaveOrUpdateItemAsync(User.Adapt<Web>());
                        ViewModelLocator.MainPageViewModel.AddItem(temp_item.Adapt<WebDto>());
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Site or Password field is or are empty.", "OK");
                        return;
                    }
                }
                else
                {
                    await _passwordRepository.SaveOrUpdateItemAsync(await Update(User));
                    ViewModelLocator.MainPageViewModel.UpdateItem(User.Adapt<WebDto>());
                }
                await Shell.Current.GoToAsync("..");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
           
        }
        public void ClearFields()
        {
            //User.Reset();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                User = query["user"] as WebDto;
                IsEditPage = (bool)query["isedit"];
                setFields();
            }
        }
        private void setFields()
        {
            if (IsEditPage && CanOpen)
            {
               
                User.Url = _current.Url;
            }
        }
        public async Task<Web> Update(WebDto m)
        {
            Web user = await _passwordRepository.GetItemByIdAsync(m.Id);
            
            user.Url = User.Url;
            return user;
        }

    }
}
