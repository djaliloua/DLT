using DataBaseContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation
{
    public class GenericRepositoryViewModel<TSource, TDestination> : GenericRepository<TSource>, IRepositoryViewModel<TSource, TDestination> where TSource : class
    {
        public virtual IList<TDestination> GetAllToViewModel()
        {
            List< TDestination > result = new List<TDestination>();
            foreach (var item in _table)
            {
                result.Add(item.ToVM<TSource, TDestination>());
            }
            return result;
        }
        public virtual void Update(TDestination entity)
        {
            TSource obj = entity.FromVM<TDestination, TSource>();
            _dbContext.Update(obj);
            _dbContext.SaveChanges();
        }
        public virtual TDestination Save(TDestination entity)
        {
            TSource obj = entity.FromVM<TDestination, TSource>();
            _table.Add(obj);
            _dbContext.SaveChanges();
            return obj.ToVM<TSource, TDestination>();
        }
    }
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        private bool disposedValue;
        protected DbContext _dbContext;
        protected DbSet<T> _table;
        public GenericRepository()
        {
            _dbContext = new RepositoryContext();
            _table = _dbContext.Set<T>();
            if (OperatingSystem.IsAndroid())
            {
                _dbContext.Database.EnsureCreated();
            }
        }

        public virtual void Delete(object id)
        {
            T obj = GetValue(id);
            _table.Remove(obj);
            _dbContext.SaveChanges();
        }

        public IList<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetValue(object id)
        {
            return _table.Find(id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _dbContext != null)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        ~GenericRepository()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Save(T entity)
        {
            _table.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(object id)
        {
            object obj = await _table.FindAsync(id);
            _table.Remove((T)obj);
            await _dbContext.SaveChangesAsync();
        }

        public Task<T> GetValueAsync(object id)
        {
            return Task.FromResult(GetValue(id));
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public Task<T> SaveAsync(T entity)
        {
            _table.Add(entity);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(entity);
        }
    }
}
