﻿using ManagPassWord.DataAcessLayer.Abstractions;
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
        private readonly IPasswordRepository _passwordRepository;

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        #endregion

        #region Constructor
        public MainPageViewModel(IPasswordRepository _db)
        {
            _passwordRepository = _db;
            load();
            CommandSetup();
        }
        #endregion

        #region Private methods
        private async void load()
        {
            await Task.Run(LoadItems);
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
                            { "user", new UserDTO() },
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

        public override async Task LoadItems()
        {
            ShowActivity();
            var repo = await _passwordRepository.GetAllItemsAsync();
            var data = repo.Adapt<List<UserDTO>>();
            SetItems(data);
            HideActivity();
        }
        
       
        
    }
}
