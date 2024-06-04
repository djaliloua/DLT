using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.MVVM.ViewModels.AccountPage;

namespace PurchaseManagement.ServiceLocator
{
    public class ViewModelLocator
    {
        //AccountAnalyticViewModel
        public static AboutViewModel AboutViewModel => GetService<AboutViewModel>();
        public static SettingsViewModel SettingsViewModel => GetService<SettingsViewModel>();
        public static AccountAnalyticViewModel AccountAnalyticViewModel => GetService<AccountAnalyticViewModel>();
        public static AccountHeaderViewModel AccountHeaderViewModel => GetService<AccountHeaderViewModel>();
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
        public static MainViewModel GetMainViewModel() => GetService<MainViewModel>();
        public static AccountListViewViewModel AccountListViewViewModel => GetService<AccountListViewViewModel>();
        public static AccountPageViewModel AccountPageViewModel => GetService<AccountPageViewModel>();
        public static MainViewModel MainViewModel => GetService<MainViewModel>();
        public static PurchaseItemsViewModel GetPurchaseItemsViewModel() => GetService<PurchaseItemsViewModel>();
        public static PurchaseItemsViewModel PurchaseItemsViewModel => GetService<PurchaseItemsViewModel>();
        public static PurchaseItemDetailsViewModel GetPurchaseItemDetailsViewModel() => GetService<PurchaseItemDetailsViewModel>();
        public static PurchaseItemDetailsViewModel PurchaseItemDetailsViewModel => GetService<PurchaseItemDetailsViewModel>();
        public static MarketFormViewModel MarketFormViewModel => GetService<MarketFormViewModel>();
    }
    
}
