using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.DataAccessLayer
{
    
    public interface IRepository
    {
        Task<MVVM.Models.Location> GetMarketLocationAsync(int purchase_id, int purchase_item_id);
        Task<int> SaveAndUpdateLocationAsync(MVVM.Models.Location location);
        Task<IList<Purchase>> GetAllPurchases();
        Task<IList<Product>> GetAllPurchaseItemById(int purchase_id);
        Task<Purchase> SavePurchaseAsync(Purchase purchase);
        Task<Product> SavePurchaseItemAsync(Product purchase_item);
        Task<Purchase> GetPurchasesByDate(DateTime dt);
        Task<Purchase> GetFullPurchaseByDate(DateTime dt);
        Task<PurchaseStatistics> SavePurchaseStatisticsItemAsyn(Purchase purchase, PurchaseStatistics purchaseStatistics);
        Task<double> GetTotalValue(Purchase purchases, string colname);
        Task<PurchaseStatistics> GetPurchaseStatistics(int id);
        Task<int> CountPurchaseItems(int purchase_id);
        Task<int> DeletePurchaseItemAsync(Product purchase);
    }
}
