using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.ExtensionMethods;
using PurchaseManagement.DataAccessLayer.Abstractions;

namespace PurchaseManagement.Utilities
{
    public static class ViewModelUtility
    {
        private static readonly IPurchaseRepositoryApi _genericRepositoryApi;
        static ViewModelUtility()
        {
            _genericRepositoryApi = ViewModelLocator.GetService<IPurchaseRepositoryApi>();
        }
        public static async Task<int> SaveAndUpdateUI(PurchaseDto purchase)
        {
            Purchase purchaseB = await _genericRepositoryApi.SaveOrUpdate(purchase.FromDto());
            PurchaseDto p = purchaseB.ToDto();
            ViewModelLocator.PurchasesListViewModel.SaveOrUpdateItem(p);
            return purchaseB.ProductStatistics.PurchaseCount;
        }
    }
}
