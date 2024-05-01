using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using ManagPassWord.ViewModels;
using ManagPassWord.ViewModels.Debt;
using ManagPassWord.ViewModels.Password;
using ManagPassWord.Views;

namespace ManagPassWord.ExtensionMethods
{
    public static class Extensions
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<SettingPage>();
            mauiAppBuilder.Services.AddTransient<SearchPage>();
            mauiAppBuilder.Services.AddSingleton<AddPassworPage>();
            mauiAppBuilder.Services.AddSingleton<DetailPage>();
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<DebtPage>();
            mauiAppBuilder.Services.AddSingleton<DebtDetailsPage>();
            mauiAppBuilder.Services.AddSingleton<AboutPage>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingView>();
            mauiAppBuilder.Services.AddSingleton<PasswordSettingView>();
            mauiAppBuilder.Services.AddSingleton<DebtSettingPage>();
            mauiAppBuilder.Services.AddSingleton<DebtFormPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelExtension(this MauiAppBuilder mauiAppBuilder)
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
