﻿using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using MVVM;
using Patterns.Abstractions;
using Patterns.Implementations;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Password
{
    public class LoadPasswordService : ILoadService<PasswordDto>
    {
        public IList<PasswordDto> Reorder(IList<PasswordDto> items)
        {
            return items;
        }
    }
    public class DetailViewModel : Loadable<PasswordDto>, IQueryAttributable
    {
        private readonly IPasswordRepository _passwordRepository;
        private WebDto _userDetail;
        public WebDto UserDetail
        {
            get => _userDetail;
            set => UpdateObservable(ref _userDetail, value, async() =>
            {
                ShowActivity();
                await Task.Run(async () => await LoadItems(value.Passwords));
                HideActivity();
            });
        }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public DetailViewModel(IPasswordRepository db, ILoadService<PasswordDto> loadService):base(loadService)
        {
            _passwordRepository = db;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
        }
        
        private async void OnDelete(object sender)
        {
            PasswordDto tempPassword = sender as PasswordDto;
            if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
            {
                if (tempPassword.Id != 0)
                {
                    if(await _passwordRepository.GetItemByUrl(tempPassword?.Web.Url) is Web web)
                    {
                        web.DeletePasswordItem(tempPassword);
                        await _passwordRepository.SaveOrUpdateItemAsync(web);
                        ViewModelLocator.MainPageViewModel.UpdateItem(web.Adapt<WebDto>());
                    }
                }
            }
        }
        private async void OnEdit(object sender)
        {
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "password", sender },
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
