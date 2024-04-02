﻿using MVVM;

namespace ManagPassWord.ViewModels
{
    public class SettingViewModel:BaseViewModel
    {
        private bool _IsPassWordSettingVisible;
        public bool IsPassWordSettingVisible
        {
            get => _IsPassWordSettingVisible;
            set => UpdateObservable(ref _IsPassWordSettingVisible, value, () =>
            {

            });
        }
        public SettingViewModel()
        {
            IsPassWordSettingVisible = false;
        }
    }
}
