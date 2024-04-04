using ManagPassWord.ViewModels.Password;
using MVVM;

namespace ManagPassWord.ViewModels
{
    public class SettingViewModel:BaseViewModel
    {
        private bool _IsPassWordSettingVisible;
        public bool IsPassWordSettingVisible
        {
            get => _IsPassWordSettingVisible;
            set => UpdateObservable(ref _IsPassWordSettingVisible, value);
        }
        public PasswordSettingViewModel PasswordSettingVM { get; } = ServiceLocator.GetService<PasswordSettingViewModel>();
        public SettingViewModel SettingVM { get; } = ServiceLocator.GetService<SettingViewModel>();
        public SettingViewModel()
        {
            IsPassWordSettingVisible = false;
        }
    }
}
