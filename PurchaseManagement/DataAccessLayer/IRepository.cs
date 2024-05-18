using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.DataAccessLayer
{
    //public interface IRepository
    //{
    //    //Task<Purchase_Items> GetPurchaseItemsAsync();
    //    MarketLocation GetMarketLocationAsync(int purchase_id, int purchase_item_id);
    //    int SaveAndUpdateLocationAsync(MarketLocation location);
    //    IList<Purchases> GetAllPurchases();
    //    IList<Purchase_Items> GetAllPurchaseItemById(int purchase_id);
    //    Purchases SavePurchaseAsync(Purchases purchase);
    //    int SavePurchaseItemAsync(Purchase_Items purchase_item);
    //    Purchases GetPurchasesByDate(DateTime dt);
    //    int SavePurchaseStatisticsItemAsyn(Purchases purchase, PurchaseStatistics purchaseStatistics);
    //    double GetTotalValue(Purchases purchases, string colname);
    //    PurchaseStatistics GetPurchaseStatistics(int id);
    //    int CountPurchaseItems(int purchase_id);
    //    int DeletePurchaseItemAsync(Purchase_Items purchase);
    //}
    public interface IRepository
    {
        //Task<Purchase_Items> GetPurchaseItemsAsync();
        Task<MarketLocation> GetMarketLocationAsync(int purchase_id, int purchase_item_id);
        Task<int> SaveAndUpdateLocationAsync(MarketLocation location);
        Task<IList<Purchases>> GetAllPurchases();
        Task<IList<Purchase_Items>> GetAllPurchaseItemById(int purchase_id);
        Task<Purchases> SavePurchaseAsync(Purchases purchase);
        Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item);
        Task<Purchases> GetPurchasesByDate(DateTime dt);
        Task<int> SavePurchaseStatisticsItemAsyn(Purchases purchase, PurchaseStatistics purchaseStatistics);
        Task<double> GetTotalValue(Purchases purchases, string colname);
        Task<PurchaseStatistics> GetPurchaseStatistics(int id);
        Task<int> CountPurchaseItems(int purchase_id);
        Task<int> DeletePurchaseItemAsync(Purchase_Items purchase);
    }
}
