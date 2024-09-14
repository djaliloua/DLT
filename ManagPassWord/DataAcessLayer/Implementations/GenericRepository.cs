using ManagPassWord.MVVM.Models;
using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.DataAcessLayer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ManagPassWord.DataAcessLayer.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : BaseEntity, new()
    {
        protected RepositoryContext _context;
        private bool _isDisposed;
        protected DbSet<T> _table;
       
        public GenericRepository()
        {
            _context = Resolver.GetService<RepositoryContext>();
            _table = _context.Set<T>();
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
            if (item.Id != 0)
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
                _context.Update(item);
            }
            else
            {
                _table.Add(item);
            }
            await _context.SaveChangesAsync();
            return item;
        }
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            _isDisposed = true;
        }

    }
}
