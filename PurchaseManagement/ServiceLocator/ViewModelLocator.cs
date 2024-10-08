using PurchaseManagement.MVVM.ViewModels.PurchasePage;
using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.MVVM.ViewModels.AccountPage;

namespace PurchaseManagement.ServiceLocator
{
    public class ViewModelLocator
    {
        public static ProductListViewModel ProductListViewModel => GetService<ProductListViewModel>();
        public static PurchasesListViewModel PurchasesListViewModel => GetService<PurchasesListViewModel>();
        public static AccountFormViewModel AccountFormViewModel => GetService<AccountFormViewModel>();
        public static ProductAnalyticsViewModel ProductAnalyticsViewModel => GetService<ProductAnalyticsViewModel>();   
        public static AboutViewModel AboutViewModel => GetService<AboutViewModel>();
        public static SettingsViewModel SettingsViewModel => GetService<SettingsViewModel>();
        public static AccountAnalyticViewModel AccountAnalyticViewModel => GetService<AccountAnalyticViewModel>();
        public static AccountHeaderViewModel AccountHeaderViewModel => GetService<AccountHeaderViewModel>();
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
        public static MainViewModel GetMainViewModel() => GetService<MainViewModel>();
        public static AccountListViewViewModel AccountListViewViewModel => GetService<AccountListViewViewModel>();
        public static AccountPageViewModel AccountPageViewModel => GetService<AccountPageViewModel>();
        public static MainViewModel MainViewModel => GetService<MainViewModel>();
        public static ProductItemsViewModel GetPurchaseItemsViewModel() => GetService<ProductItemsViewModel>();
        public static ProductItemsViewModel ProductItemsViewModel => GetService<ProductItemsViewModel>();
        public static PurchaseItemDetailsViewModel GetPurchaseItemDetailsViewModel() => GetService<PurchaseItemDetailsViewModel>();
        public static PurchaseItemDetailsViewModel PurchaseItemDetailsViewModel => GetService<PurchaseItemDetailsViewModel>();
        public static MarketFormViewModel MarketFormViewModel => GetService<MarketFormViewModel>();
    }
    
}
