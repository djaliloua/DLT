using CommunityToolkit.Maui;
using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using ManagPassWord.ViewModels;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.ViewModels.Password;
using ManagPassWord.Views;

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
            mauiAppBuilder.Services.AddSingleton<SettingPage>();
            mauiAppBuilder.Services.AddTransient<SearchPage>();
            mauiAppBuilder.Services.AddSingleton<AddPassworPage>();
            mauiAppBuilder.Services.AddSingleton<DetailPage>();
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<DebtPage>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsPage>();
            mauiAppBuilder.Services.AddSingleton<AboutPage>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingView>();
            mauiAppBuilder.Services.AddSingleton<PasswordSettingView>();
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
            //Repositories
            mauiAppBuilder.Services.AddSingleton<IRepository<User>, UserRepository>();
            mauiAppBuilder.Services.AddSingleton<IRepository<DebtModel>, DebtRepository>();

            return mauiAppBuilder;
        }
    }
}
