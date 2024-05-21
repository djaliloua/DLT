using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.DataAccessLayer
{
    
    public interface IRepository
    {
        //Task<Purchase_Items> GetPurchaseItemsAsync();
        Task<MarketLocation> GetMarketLocationAsync(int purchase_id, int purchase_item_id);
        Task<int> SaveAndUpdateLocationAsync(MarketLocation location);
        Task<IList<Purchases>> GetAllPurchases();
        Task<IList<Purchase_Items>> GetAllPurchaseItemById(int purchase_id);
        Task<Purchases> SavePurchaseAsync(Purchases purchase);
        Task<Purchase_Items> SavePurchaseItemAsync(Purchase_Items purchase_item);
        Task<Purchases> GetPurchasesByDate(DateTime dt);
        Task<PurchaseStatistics> SavePurchaseStatisticsItemAsyn(Purchases purchase, PurchaseStatistics purchaseStatistics);
        Task<double> GetTotalValue(Purchases purchases, string colname);
        Task<PurchaseStatistics> GetPurchaseStatistics(int id);
        Task<int> CountPurchaseItems(int purchase_id);
        Task<int> DeletePurchaseItemAsync(Purchase_Items purchase);
    }
}
