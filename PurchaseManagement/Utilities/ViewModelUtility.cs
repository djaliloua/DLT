using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using Mapster;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        private static readonly IGenericRepositoryApi _genericRepositoryApi;
        static ViewModelUtility()
        {
            _genericRepositoryApi = ViewModelLocator.GetService<IGenericRepositoryApi>();
        }
        public static async Task<int> SaveAndUpdateUI(Purchase purchase)
        {
            Purchase purchaseApi = await _genericRepositoryApi.SaveOrUpdate(purchase);
            PurchaseDto p = purchaseApi.Adapt<PurchaseDto>();
            ViewModelLocator.MainViewModel.SaveOrUpdateItem(p);
            return purchaseApi.ProductStatistics.PurchaseCount;
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
