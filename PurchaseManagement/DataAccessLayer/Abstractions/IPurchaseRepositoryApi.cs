using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IPurchaseRepositoryApi
    {
        Task Delete(int id);
        Task<Purchase> GetById(int id);
        Task<Purchase> GetByDate(string dt);
        Task<Purchase> SaveOrUpdate(Purchase purchase);
        Task<List<Purchase>> GetAllItems();
        Task<PurchaseDto> RemoveProduct(Product product);
    }
}
