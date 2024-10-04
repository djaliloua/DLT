using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.ExtensionMethods;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        public static async Task<int> SaveAndUpdateUI(Purchase purchase)
        {
            using var repo = new PurchaseRepository();
            Purchase purchaseB = await repo.SaveOrUpdateItemAsync(purchase);
            PurchaseDto p = purchaseB.ToDto();
            ViewModelLocator.MainViewModel.SaveOrUpdateItem(p);
            return purchaseB.ProductStatistics.PurchaseCount;
        }
        
    }
}
