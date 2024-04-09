﻿using MVVM;

namespace ManagPassWord.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Version { get; private set; }
        public AboutViewModel()
        {
            Version = AppInfo.Current.VersionString;
        }
    }
}
