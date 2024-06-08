using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API.DataAccessLayer
{
    public class ProductStatisticRepository:IRepository<ProductStatistics>
    {
        private readonly RepositoryContext _context;
        public ProductStatisticRepository(RepositoryContext context)
        {
            _context = context;
        }
        public async Task<ProductStatistics> Get(int id)
        {
            var purchaseStat = await _context.ProductStatistics.FindAsync(id);
            return purchaseStat;
        }

        public async Task<IEnumerable<ProductStatistics>> GetAll()
        {
            return await _context.ProductStatistics.ToListAsync();
        }

        public bool ItemExist(int id)
        {
            return _context.ProductStatistics.Any(e => e.Id == id);
        }

        public async Task<ProductStatistics> Post(ProductStatistics item)
        {
            _context.ProductStatistics.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task Delete(int id)
        {
            var purchaseStat = await _context.ProductStatistics.FindAsync(id);
            _context.ProductStatistics.Remove(purchaseStat);
            await _context.SaveChangesAsync();
        }
        public async Task Put(int id, ProductStatistics item)
        {
            if (id != item.Id)
            {
                return;
            }
            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}
