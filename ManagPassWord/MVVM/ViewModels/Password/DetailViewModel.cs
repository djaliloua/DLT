﻿using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class DetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IPasswordRepository _db;
        private WebDto _userDetail;
        public WebDto UserDetail
        {
            get => _userDetail;
            set => UpdateObservable(ref _userDetail, value);
        }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public DetailViewModel(IPasswordRepository db)
        {
            _db = db;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(delete);
        }
        private async void delete(object sender)
        {
            if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
            {
                if (UserDetail.Id != 0)
                {
                    await _db.DeleteItemAsync(UserDetail.Adapt<Web>());
                    ViewModelLocator.MainPageViewModel.DeleteItem(UserDetail);
                }
                await Shell.Current.GoToAsync("..");
            }

        }
        private async void OnEdit(object sender)
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
                UserDetail = query["user"] as WebDto;
            }
        }
    }
}
