using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.ViewModel;
using PurchaseManagement.DataAccessLayer.Repository;
using Models.Market;
using Repository;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        public static async Task<int> SaveAndUpdateUI(Purchase purchase)
        {
            using var repo = new PurchaseRepository();
            Purchase purchaseB = await repo.SaveAsync(purchase);
            PurchaseViewModel p = purchaseB.ToVM<Purchase, PurchaseViewModel>();
            ViewModelLocator.PurchasesListViewModel.SaveOrUpdateItem(p);
            return purchaseB.ProductStatistics.PurchaseCount;
        }
        
    }
}
