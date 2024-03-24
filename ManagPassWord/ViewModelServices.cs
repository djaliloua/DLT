using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using ManagPassWord.ViewModels;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.ViewModels.Password;

namespace ManagPassWord
{
    public static class ViewModelServices
    {
        public static MainPageViewModel GetMainPageViewModel() => ServiceLocator.GetService<MainPageViewModel>();
        public static DebtDetailsViewModel GetDebtDetailsViewModel() => ServiceLocator.GetService<DebtDetailsViewModel>();
        public static DebtPageViewModel GetDebtPageViewModel() => ServiceLocator.GetService<DebtPageViewModel>();
        public static AddPasswordViewModel GetAddPassordViewModel() => ServiceLocator.GetService<AddPasswordViewModel>();
        public static SearchViewModel GetSearchViewModel() => ServiceLocator.GetService<SearchViewModel>();
        public static AboutViewModel GetAboutViewModel() => ServiceLocator.GetService<AboutViewModel>();
        public static DetailViewModel GetDetailViewModel() => ServiceLocator.GetService<DetailViewModel>();
        public static SettingViewModel GetSettingViewModel() => ServiceLocator.GetService<SettingViewModel>();

        public static MauiAppBuilder RegisterViewModel(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<SettingPage>();
            mauiAppBuilder.Services.AddTransient<SearchPage>();
            mauiAppBuilder.Services.AddSingleton<AddPassworPage>();
            mauiAppBuilder.Services.AddSingleton<DetailPage>();
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<DebtPage>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsPage>();
            mauiAppBuilder.Services.AddSingleton<AboutPage>();
            // ViewModels
            mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
            mauiAppBuilder.Services.AddSingleton<AddPasswordViewModel>();
            mauiAppBuilder.Services.AddTransient<SettingViewModel>();
            mauiAppBuilder.Services.AddTransient<SearchViewModel>();
            mauiAppBuilder.Services.AddSingleton<DetailViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtPageViewModel>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AboutViewModel>();
            //Repositories
            mauiAppBuilder.Services.AddSingleton<IRepository<User>, UserRepository>();
            mauiAppBuilder.Services.AddSingleton<IRepository<DebtModel>, DebtRepository>();

            return mauiAppBuilder;
        }
    }
}
