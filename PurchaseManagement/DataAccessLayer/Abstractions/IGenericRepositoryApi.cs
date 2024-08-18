using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IGenericRepositoryApi
    {
        Task Delete(int id);
        Task<Purchase> GetById(int id);
        Task<Purchase> GetByDate(string dt);
        Task<Purchase> SaveOrUpdate(Purchase purchase);
        Task<IEnumerable<Purchase>> GetAllItems();

    }
}
