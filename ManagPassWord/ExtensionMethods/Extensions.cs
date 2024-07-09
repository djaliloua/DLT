using ManagPassWord.DataAcessLayer.Implementations;
using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.Pages;
using Mapster;
using ManagPassWord.Pages.Debt;
using ManagPassWord.MVVM.ViewModels;
using ManagPassWord.MVVM.ViewModels.Debt;
using ManagPassWord.MVVM.ViewModels.Password;
using ManagPassWord.MVVM.Views;
using ManagPassWord.DataAcessLayer.Contexts;

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

            
            //
            

            return mauiAppBuilder;
        }
        public static MauiAppBuilder ContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<RepositoryContext>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RepositoryExtension(this MauiAppBuilder mauiAppBuilder)
        {
            //Repositories
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<User>, GenericRepository<User>>();
            mauiAppBuilder.Services.AddTransient<IGenericRepository<DebtModel>, GenericRepository<DebtModel>>();
            mauiAppBuilder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder CommonsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddMapster();
            return mauiAppBuilder;
        }
    }
}
