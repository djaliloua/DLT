using CameraApp.DataAccess.Abstraction;
using CameraApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CameraApp.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : BaseEntity
    {
        private bool _isDisposed;
        public RepositoryContext _context;
        public GenericRepository()
        {
            _context = new RepositoryContext();
            _context.Database.EnsureCreated();
        }
        public virtual void DeleteItem(T item)
        {
            T trackedItem = _context.Set<T>().Find(item.Id);
            if (trackedItem != null)
            {
                _context.Remove(trackedItem);
                _context.SaveChanges();
            }
        }

        public virtual async Task DeleteItemAsync(T item)
        {
            T trackedItem = await _context.Set<T>().FindAsync(item.Id);
            if (trackedItem != null)
            {
                _context.Remove(trackedItem);
                await _context.SaveChangesAsync();
            }
        }

        public virtual List<T> GetAllItems()
        {
            return _context.Set<T>().ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual T GetItemById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetItemByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual T SaveOrUpdateItem(T item)
        {
            if (_context is RepositoryContext || _isDisposed)
                _context = new RepositoryContext();

            if (item.Id != 0)
            {
                _context.Update(item);
            }
            else
            {
                _context.Set<T>().Add(item);
            }
            _context.SaveChanges();
            return item;
        }

        public virtual async Task<T> SaveOrUpdateItemAsync(T item)
        {
            try
            {
                using RepositoryContext context = new RepositoryContext();
                if (item.Id != 0)
                {
                    context.Update(item);
                }
                else
                {
                    context.Set<T>().Add(item);
                }
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return item;
        }
        //The following Method is going to Dispose of the Context Object
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            _isDisposed = true;
        }
        //The following Method is going to Dispose of the Context Object

        ~GenericRepository()
        {

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
