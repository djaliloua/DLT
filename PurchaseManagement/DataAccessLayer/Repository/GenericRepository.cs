using PurchaseManagement.MVVM.Models;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.DataAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected RepositoryContext _context;
        protected DbSet<T> _table;
        public GenericRepository(RepositoryContext context)
        {
            _context = context;
            _table = _context.Set<T>();
            _context.Database.EnsureCreated();
        }
        public GenericRepository()
        {
            _context = ServiceLocator.ViewModelLocator.GetService<RepositoryContext>();
            _table = _context.Set<T>();
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        public virtual void DeleteItem(T item)
        {
            T trackedItem = _table.Find(item.Id);
            if (trackedItem != null)
            {
                _context.Remove(trackedItem);
                _context.SaveChanges();
            }
        }

        public virtual async Task DeleteItemAsync(T item)
        {
            T trackedItem = await _table.FindAsync(item.Id);
            if (trackedItem != null)
            {
                _context.Remove(trackedItem);
                await _context.SaveChangesAsync();
            }
        }

        public virtual IEnumerable<T> GetAllItems()
        {
            return _table.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            return await _table.ToListAsync();
        }

        public virtual T GetItemById(int id)
        {
            return _table.Find(id);
        }

        public async Task<T> GetItemByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        public virtual T SaveOrUpdateItem(T item)
        {
            if(item.Id != 0)
            {
                _context.Entry(item).OriginalValues.SetValues(item);
            }
            else
            {
                _table.Add(item);
            }
            _context.SaveChanges();
            return item;
        }

        public virtual async Task<T> SaveOrUpdateItemAsync(T item)
        {
            if (item.Id != 0)
            {
                _context.Entry(item).OriginalValues.SetValues(item);
            }
            else
            {
                _table.Add(item);
            }
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
