using Models.Market;
using Repository.Interface;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IList<Product> GetAllItemById(int id);
        Task<IList<Product>> GetAllItemByIdAsync(int id);
    }
}
