using PurchaseManagement.MVVM.ViewModels;

namespace PurchaseManagement.Services
{
    public static class RegisterViewModels
    {
        public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        public static PurchaseItemsViewModel GetPurchaseItemsViewModel() => ServiceLocator.GetService<PurchaseItemsViewModel>();
        //public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        //public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        //public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        public static MauiAppBuilder RegisterViewModel(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();//PurchaseItemsViewModel
            mauiAppBuilder.Services.AddSingleton<PurchaseItemsViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            return mauiAppBuilder;
        }
    }
}
