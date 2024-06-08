using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API.DataAccessLayer
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly RepositoryContext _context;
        public ProductRepository(RepositoryContext context)
        {
            _context = context;
        }
        public async Task<Product> Get(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public bool ItemExist(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<Product> Post(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task Put(int id, Product item)
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
