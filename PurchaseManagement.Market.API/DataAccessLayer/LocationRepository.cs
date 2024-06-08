using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API.DataAccessLayer
{
    public class LocationRepository : IRepository<Location>
    {
        private readonly RepositoryContext _context;
        public LocationRepository(RepositoryContext context)
        {
            _context = context;
        }
        public async Task<Location> Get(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            return location;
        }

        public async Task<IEnumerable<Location>> GetAll()
        {
            return await _context.Locations.ToListAsync();
        }

        public bool ItemExist(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }

        public async Task<Location> Post(Location item)
        {
            _context.Locations.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task Delete(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
        public async Task Put(int id, Location item)
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
