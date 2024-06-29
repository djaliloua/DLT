using PurchaseManagement.MVVM.ViewModels;
using CommunityToolkit.Maui;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using Mapster;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.Pages;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.DTOs;
using CommunityToolkit.Maui.Storage;
using MauiNavigationHelper.NavigationLib.Services;
using MauiNavigationHelper.NavigationLib.Abstractions;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.Commons.ExportFileStrategies;


namespace PurchaseManagement.ExtensionMethods
{
    
    public static class Extensions
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            mauiAppBuilder.Services.AddScoped<AccountAnalyticPage>();
            mauiAppBuilder.Services.AddScoped<ProductAnalytics>();
            mauiAppBuilder.Services.AddScoped<ProductsPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RepositoryExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            mauiAppBuilder.Services.AddSingleton<IProductRepository, ProductRepository>();
            mauiAppBuilder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<MarketModels.Location>, GenericRepository<MarketModels.Location>>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<ProductStatistics>, GenericRepository<ProductStatistics>>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<Product>, GenericRepository<Product>>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<Purchase>, GenericRepository<Purchase>>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<Account>, GenericRepository<Account>>();

            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<AboutViewModel>();
            mauiAppBuilder.Services.AddScoped<SettingsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AccountAnalyticViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountListViewViewModel>();
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            mauiAppBuilder.Services.AddSingleton<ProductItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddTransient<MarketFormViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountPageViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountHeaderViewModel>();
            mauiAppBuilder.Services.AddScoped<ProductAnalyticsViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder UtilityExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            mauiAppBuilder.Services.AddScoped<ExportContext<AccountDTO>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<AccountDTO>, AccountTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<ExportContext<ProductDto>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<ProductDto>, ProductTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<INavigationService, NavigationService>();
            mauiAppBuilder.Services.AddMapster();
            return mauiAppBuilder;
        }
    }
}
