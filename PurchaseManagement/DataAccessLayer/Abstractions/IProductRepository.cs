using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IList<Product>> GetAllItemById(int id);
    }
}
