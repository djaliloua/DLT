using PurchaseManagement.MVVM.ViewModels;

namespace PurchaseManagement.Services
{
    public static class RegisterViewModels
    {
        public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        public static PurchaseItemsViewModel GetPurchaseItemsViewModel() => ServiceLocator.GetService<PurchaseItemsViewModel>();
        public static PurchaseItemDetailsViewModel GetPurchaseItemDetailsViewModel() => ServiceLocator.GetService<PurchaseItemDetailsViewModel>();
        public static PurchaseFormViewModel GetPurchaseFormViewModel() => ServiceLocator.GetService<PurchaseFormViewModel>();
        //public static MainViewModel GetMainViewModel() => ServiceLocator.GetService<MainViewModel>();
        public static MauiAppBuilder RegisterViewModel(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();//PurchaseItemsViewModel
            mauiAppBuilder.Services.AddSingleton<PurchaseItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseFormViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            return mauiAppBuilder;
        }
    }
}
