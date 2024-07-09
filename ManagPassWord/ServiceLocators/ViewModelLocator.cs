using ManagPassWord.MVVM.ViewModels.Debt;
using ManagPassWord.MVVM.ViewModels;
using ManagPassWord.MVVM.ViewModels.Password;

namespace ManagPassWord.ServiceLocators
{
    public static class ViewModelLocator
    {
        public static MainPageViewModel MainPageViewModel => Resolver.GetService<MainPageViewModel>();
        public static DebtDetailsViewModel DebtDetailsViewModel => Resolver.GetService<DebtDetailsViewModel>();
        public static DebtPageViewModel DebtPageViewModel => Resolver.GetService<DebtPageViewModel>();
        public static AddPasswordViewModel AddPasswordViewModel => Resolver.GetService<AddPasswordViewModel>();
        public static SearchViewModel SearchViewModel => Resolver.GetService<SearchViewModel>();
        public static AboutViewModel AboutViewModel => Resolver.GetService<AboutViewModel>();
        public static DetailViewModel DetailViewModel => Resolver.GetService<DetailViewModel>();
        public static SettingViewModel SettingViewModel => Resolver.GetService<SettingViewModel>();
        public static DebtSettingViewModel DebtSettingViewModel => Resolver.GetService<DebtSettingViewModel>();
        public static PasswordSettingViewModel PasswordSettingViewModel => Resolver.GetService<PasswordSettingViewModel>();
        public static DebtFormViewModel DebtFormViewModel => Resolver.GetService<DebtFormViewModel>();


    }
}
