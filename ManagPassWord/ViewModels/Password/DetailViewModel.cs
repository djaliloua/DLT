using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Password
{
    public class DetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IRepository<User> _db;
        private User _userDetail;
        public User UserDetail
        {
            get => _userDetail;
            set => UpdateObservable(ref _userDetail, value);
        }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public DetailViewModel(IRepository<User> db)
        {
            _db = db;
            EditCommand = new Command(On_Edit);
            DeleteCommand = new Command(delete);
        }
        private async void delete(object sender)
        {
            if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
            {
                if (UserDetail.Id != 0)
                {
                    await _db.DeleteById(UserDetail);
                }
                await Shell.Current.GoToAsync("..");
                await ViewModelServices.MainPageViewModel.Load();
            }

        }
        private async void On_Edit(object sender)
        {
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", UserDetail },
                            { "isedit", true }
                        };
            await Shell.Current.GoToAsync(nameof(AddPassworPage), navigationParameter);
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                UserDetail = query["user"] as User;
            }
        }
    }
}
