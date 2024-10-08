using PurchaseManagement.MVVM.ViewModels;
using CommunityToolkit.Maui;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
using PurchaseManagement.DataAccessLayer.Repository;
using Mapster;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.ViewModels.PurchasePage;
using PurchaseManagement.Pages;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.DTOs;
using CommunityToolkit.Maui.Storage;
using MauiNavigationHelper.NavigationLib.Services;
using MauiNavigationHelper.NavigationLib.Abstractions;
using PurchaseManagement.Commons.ExportFileStrategies;
using PurchaseManagement.DataAccessLayer.Contexts;
using Patterns.Abstractions;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.Accounts;


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
    public static class MapperExtension
    {
        public static IList<PurchaseDto> ToDto(this List<Purchase> items) => items.Adapt<List<PurchaseDto>>();
        public static IList<AccountDTO> ToDto(this IEnumerable<Account> items) => items.Adapt<List<AccountDTO>>();
        public static PurchaseDto ToDto(this Purchase item) => item.Adapt<PurchaseDto>();
        public static AccountDTO ToDto(this Account item) => item.Adapt<AccountDTO>();
        public static ProductDto ToDto(this Product item) => item.Adapt<ProductDto>();
        public static LocationDto ToDto(this ProductLocation item) => item.Adapt<LocationDto>();
        public static Product FromDto(this ProductDto item) => item.Adapt<Product>();
        public static Purchase FromDto(this PurchaseDto item) => item.Adapt<Purchase>();
        public static Account FromDto(this AccountDTO item) => item.Adapt<Account>();
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
            mauiAppBuilder.Services.AddSingleton<IPurchaseRepositoryApi, PurchaseRepositoryApi>();
            mauiAppBuilder.Services.AddTransient<IAccountRepositoryApi, AccountRepositoryApi>();
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
            mauiAppBuilder.Services.AddScoped<ExportContext<AccountDTO>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<AccountDTO>, AccountTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<ExportContext<ProductDto>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<ProductDto>, ProductTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<INavigationService, NavigationService>();
            mauiAppBuilder.Services.AddMapster();
            mauiAppBuilder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
            return mauiAppBuilder;
        }
        public static MauiAppBuilder LoadBIExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<ILoadService<AccountDTO>, LoadAccountService>();
            mauiAppBuilder.Services.AddScoped<ILoadService<PurchaseDto>, LoadPurchaseService>();
            mauiAppBuilder.Services.AddScoped<ILoadService<ProductDto>, LoadProductService>();

            return mauiAppBuilder;
        }
    }
}
