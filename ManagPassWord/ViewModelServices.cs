using CommunityToolkit.Maui;
using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.ViewModels;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.ViewModels.Password;

namespace ManagPassWord
{
    public static class ViewModelServices
    {
        //public static MainPageViewModel GetMainPageViewModel() => ServiceLocator.GetService<MainPageViewModel>();
        public static MainPageViewModel MainPageViewModel => ServiceLocator.GetService<MainPageViewModel>();
        //public static DebtDetailsViewModel GetDebtDetailsViewModel() => ServiceLocator.GetService<DebtDetailsViewModel>();
        public static DebtDetailsViewModel DebtDetailsViewModel => ServiceLocator.GetService<DebtDetailsViewModel>();
        public static DebtPageViewModel GetDebtPageViewModel() => ServiceLocator.GetService<DebtPageViewModel>();
        public static DebtPageViewModel DebtPageViewModel => ServiceLocator.GetService<DebtPageViewModel>();
        //public static AddPasswordViewModel GetAddPassordViewModel() => ServiceLocator.GetService<AddPasswordViewModel>();
        public static AddPasswordViewModel AddPasswordViewModel => ServiceLocator.GetService<AddPasswordViewModel>();
        public static SearchViewModel GetSearchViewModel() => ServiceLocator.GetService<SearchViewModel>();
        public static AboutViewModel GetAboutViewModel() => ServiceLocator.GetService<AboutViewModel>();
        public static DetailViewModel GetDetailViewModel() => ServiceLocator.GetService<DetailViewModel>();
        public static DetailViewModel DetailViewModel => ServiceLocator.GetService<DetailViewModel>();
        public static SettingViewModel GetSettingViewModel() => ServiceLocator.GetService<SettingViewModel>();
        public static DebtFormViewModel GetDebtFormViewModel() => ServiceLocator.GetService<DebtFormViewModel>();

        public static MauiAppBuilder RegisterViewModel(this MauiAppBuilder mauiAppBuilder)
        {
            // ViewModels
            mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
            mauiAppBuilder.Services.AddTransient<AddPasswordViewModel>();
            mauiAppBuilder.Services.AddSingleton<SettingViewModel>();
            mauiAppBuilder.Services.AddTransient<SearchViewModel>();
            mauiAppBuilder.Services.AddSingleton<DetailViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtPageViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AboutViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingViewModel>();
            mauiAppBuilder.Services.AddSingleton<PasswordSettingViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtFormViewModel>();

            //Repositories
            mauiAppBuilder.Services.AddSingleton<IRepository<User>, UserRepository>();
            mauiAppBuilder.Services.AddTransient<IRepository<DebtModel>, DebtRepository>();

            return mauiAppBuilder;
        }
    }
}
