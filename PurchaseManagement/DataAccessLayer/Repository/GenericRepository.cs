using PurchaseManagement.MVVM.Models;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.DataAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : BaseEntity
    {
        private bool _isDisposed;
        public RepositoryContext _context;
        public GenericRepository()
        {
            _context = new RepositoryContext();
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

        public virtual IEnumerable<T> GetAllItems()
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
            await _context.SaveChangesAsync();
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
