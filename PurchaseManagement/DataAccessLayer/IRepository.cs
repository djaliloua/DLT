using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.DataAccessLayer
{
    public interface IRepository
    {
        Task<IEnumerable<Purchases>> GetAllPurchases();
        Task<IEnumerable<Purchase_Items>> GetAllPurchaseItemById(int purchase_id);
        Task<int> SavePurchaseAsync(Purchases purchase);
        Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item);
        Task<IEnumerable<Purchases>> GetPurchasesByDate();
        Task<int> SavePurchaseStatisticsItemAsyn(PurchaseStatistics purchaseStatistics);
        Task<string> GetTotalValue(Purchases purchases, string colname);
        Task<PurchaseStatistics> GetPurchaseStatistics(int id);
        Task<string> CountPurchaseItems(int purchase_id);
    }
}
