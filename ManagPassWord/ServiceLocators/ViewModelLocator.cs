using ManagPassWord.ViewModels;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.ViewModels.Password;

namespace ManagPassWord.ServiceLocators
{
    public static class ViewModelLocator
    {
        //ViewModelServices
        //public static MainPageViewModel GetMainPageViewModel() => ServiceLocator.GetService<MainPageViewModel>();
        public static MainPageViewModel MainPageViewModel => Resolver.GetService<MainPageViewModel>();
        //public static DebtDetailsViewModel GetDebtDetailsViewModel() => ServiceLocator.GetService<DebtDetailsViewModel>();
        public static DebtDetailsViewModel DebtDetailsViewModel => Resolver.GetService<DebtDetailsViewModel>();
        public static DebtPageViewModel DebtPageViewModel => Resolver.GetService<DebtPageViewModel>();
        //public static DebtPageViewModel DebtPageViewModel => ServiceLocator_bis.GetService<DebtPageViewModel>();
        //public static AddPasswordViewModel GetAddPassordViewModel() => ServiceLocator.GetService<AddPasswordViewModel>();
        public static AddPasswordViewModel AddPasswordViewModel => Resolver.GetService<AddPasswordViewModel>();
        public static SearchViewModel SearchViewModel => Resolver.GetService<SearchViewModel>();
        public static AboutViewModel AboutViewModel => Resolver.GetService<AboutViewModel>();
        //public static DetailViewModel DetailViewModel => ServiceLocator_bis.GetService<DetailViewModel>();
        public static DetailViewModel DetailViewModel => Resolver.GetService<DetailViewModel>();
        public static SettingViewModel SettingViewModel => Resolver.GetService<SettingViewModel>();
        public static DebtSettingViewModel DebtSettingViewModel => Resolver.GetService<DebtSettingViewModel>();
        public static PasswordSettingViewModel PasswordSettingViewModel => Resolver.GetService<PasswordSettingViewModel>();
        public static DebtFormViewModel DebtFormViewModel => Resolver.GetService<DebtFormViewModel>();


    }
}
