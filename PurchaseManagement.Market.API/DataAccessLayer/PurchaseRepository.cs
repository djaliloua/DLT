using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API.DataAccessLayer
{
    public class PurchaseRepository:IRepository<Purchase>
    {
        private readonly RepositoryContext _context;
        public PurchaseRepository(RepositoryContext context)
        {
            _context = context;
        }
        public async Task<Purchase> Get(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetAll()
        {
            return await _context.Purchases.ToListAsync();
        }

        public bool ItemExist(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }

        public async Task<Purchase> Post(Purchase item)
        {
            _context.Purchases.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task Delete(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
        public async Task Put(int id, Purchase item)
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
