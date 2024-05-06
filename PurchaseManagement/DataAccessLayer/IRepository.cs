using PurchaseManagement.MVVM.Models;
using System.Collections.ObjectModel;

namespace PurchaseManagement.DataAccessLayer
{
    public interface IRepository
    {
        Task<MarketLocation> GetMarketLocationAsync(int purchase_id, int purchase_item_id);
        Task<int> SaveAndUpdateLocationAsync(MarketLocation location);
        Task<IEnumerable<Purchases>> GetAllPurchases();
        Task<IList<Purchase_Items>> GetAllPurchaseItemById(int purchase_id);
        Task<int> SavePurchaseAsync(Purchases purchase);
        Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item);
        Task<IEnumerable<Purchases>> GetPurchasesByDate(DateTime dt);
        Task<int> SavePurchaseStatisticsItemAsyn(PurchaseStatistics purchaseStatistics);
        Task<long> GetTotalValue(Purchases purchases, string colname);
        Task<PurchaseStatistics> GetPurchaseStatistics(int id);
        Task<int> CountPurchaseItems(int purchase_id);
        Task<int> DeletePurchaseItemAsync(Purchase_Items purchase);
    }
}
