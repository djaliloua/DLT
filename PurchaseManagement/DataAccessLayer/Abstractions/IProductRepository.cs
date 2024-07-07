using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IList<Product> GetAllItemById(int id);
        Task<IList<Product>> GetAllItemByIdAsync(int id);
    }
}
