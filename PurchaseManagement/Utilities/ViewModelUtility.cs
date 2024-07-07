using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using Mapster;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        private static readonly IPurchaseRepository _purchaseRepository;
        static ViewModelUtility()
        {
            _purchaseRepository = ViewModelLocator.GetService<IPurchaseRepository>();
        }
        public static async Task<int> SaveAndUpdateUI(Purchase purchase)
        {
            Purchase purchaseB = await _purchaseRepository.SaveOrUpdateItemAsync(purchase);
            PurchaseDto p = purchaseB.Adapt<PurchaseDto>();
            ViewModelLocator.MainViewModel.SaveOrUpdateItem(p);
            return purchaseB.ProductStatistics.PurchaseCount;
        }
        public static void UpdateProduct(Purchase purchase, Product product)
        {
            for (int i = 0; i < purchase.Products.Count; i++)
            {
                if (purchase.Products[i].Id == product.Id)
                    purchase.Products[i] = product;
            }
            PurchaseUtility.UpdateStatistics(purchase);
        }
    }
}
