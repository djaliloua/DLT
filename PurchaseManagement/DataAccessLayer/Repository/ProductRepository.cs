using Microsoft.EntityFrameworkCore;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        public IList<Product> GetAllItemById(int id)
        {
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Id desc;";

            return _table.FromSqlRaw(sqlCmd).ToList();
        }

        public async Task<IList<Product>> GetAllItemByIdAsync(int id)
        {
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Id desc;";
            
            return await _table.FromSqlRaw(sqlCmd).ToListAsync();
        }
        
    }
}
