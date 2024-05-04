using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.Pages;
using PurchaseManagement.DataAccessLayer;

namespace PurchaseManagement.ExtensionMethods
{
    public static class Extensions
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            mauiAppBuilder.Services.AddScoped<AccountAnalyticPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder DbContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IRepository, Repository>();
            mauiAppBuilder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            //AccountAnalyticViewModel
            mauiAppBuilder.Services.AddScoped<AboutViewModel>();
            mauiAppBuilder.Services.AddScoped<SettingsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AccountAnalyticViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountViewModel>();
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();//PurchaseItemsViewModel
            mauiAppBuilder.Services.AddSingleton<PurchaseItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddScoped<MarketFormViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            return mauiAppBuilder;
        }
    }
}
