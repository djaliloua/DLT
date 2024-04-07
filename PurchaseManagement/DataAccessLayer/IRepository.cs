using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.DataAccessLayer
{
    public interface IRepository
    {
        Task<IEnumerable<Purchases>> GetAllPurchases();
        Task<IEnumerable<Purchase_Items>> GetAllPurchaseItemById(int purchase_id);
        Task<int> SavePurchaseAsync(Purchases purchase);
        Task<int> SavePurchaseItemAsync(Purchase_Items purchase_item);
    }
}
