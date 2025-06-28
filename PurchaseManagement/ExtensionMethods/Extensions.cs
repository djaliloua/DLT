using PurchaseManagement.MVVM.ViewModels;
using CommunityToolkit.Maui;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
using PurchaseManagement.DataAccessLayer.Repository;
using Mapster;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.ViewModels.PurchasePage;
using PurchaseManagement.Pages;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.ViewModel;
using CommunityToolkit.Maui.Storage;
using MauiNavigationHelper.NavigationLib.Services;
using MauiNavigationHelper.NavigationLib.Abstractions;
using PurchaseManagement.Commons.ExportFileStrategies;
using Patterns.Abstractions;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Models.Market;
using DataBaseContexts;


namespace PurchaseManagement.ExtensionMethods
{
    public static class DictExtension
    {
        public static T GetValue<T>(this IDictionary<string, object> item, string key)
        {
            T res = default(T);
            object obj = null;
            if(item.TryGetValue(key, out obj))
            {
                res = (T)obj;
            }
            
            return res;
        }
    }
    
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
            mauiAppBuilder.Services.AddTransient<IAccountRepository, AccountRepository>();
            mauiAppBuilder.Services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<RepositoryContext>();
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
            mauiAppBuilder.Services.AddScoped<MarketFormViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountPageViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountHeaderViewModel>();
            mauiAppBuilder.Services.AddScoped<ProductAnalyticsViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountFormViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchasesListViewModel>();
            mauiAppBuilder.Services.AddSingleton<ProductListViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder UtilityExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            mauiAppBuilder.Services.AddScoped<ExportContext<AccountViewModel>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<AccountViewModel>, AccountTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<ExportContext<ProductViewModel>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<ProductViewModel>, ProductTxtStrategy>();
            mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
            mauiAppBuilder.Services.AddMapster();
            mauiAppBuilder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
            return mauiAppBuilder;
        }
        public static MauiAppBuilder LoadBIExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ILoadService<AccountViewModel>, LoadAccountService>();
            mauiAppBuilder.Services.AddScoped<ILoadService<PurchaseViewModel>, LoadPurchaseService>();
            mauiAppBuilder.Services.AddScoped<ILoadService<ProductViewModel>, LoadProductService>();

            return mauiAppBuilder;
        }
    }
}
