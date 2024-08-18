using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using Mapster;
using PurchaseManagement.DataAccessLayer.Repository;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        public static async Task<int> SaveAndUpdateUI(Purchase purchase)
        {
            using var repo = new PurchaseRepository();
            Purchase purchaseB = await repo.SaveOrUpdateItemAsync(purchase);
            PurchaseDto p = purchaseB.Adapt<PurchaseDto>();
            ViewModelLocator.MainViewModel.SaveOrUpdateItem(p);
            return purchaseB.ProductStatistics.PurchaseCount;
        }
        
    }
}
