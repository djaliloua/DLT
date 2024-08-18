using Microsoft.EntityFrameworkCore;
using PurchaseManagement.DataAccessLayer.Contexts;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        public IList<Product> GetAllItemById(int id)
        {
            using(var context = new RepositoryContext())
            {
                string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Id desc;";
                return context.Products.FromSqlRaw(sqlCmd).ToList();
            }
        }

        public async Task<IList<Product>> GetAllItemByIdAsync(int id)
        {
            using(var context = new RepositoryContext())
            {
                string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Id desc;";
                return await context.Products.FromSqlRaw(sqlCmd).ToListAsync();
            }
        }
    }
}
