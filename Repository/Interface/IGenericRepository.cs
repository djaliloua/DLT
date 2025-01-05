namespace Repository.Interface
{
    public interface IRepositoryViewModel<TSource, TDestination> : IGenericRepository<TSource>
    {
        IList<TDestination> GetAllToViewModel();
    }
    public interface IGenericRepository<T> : IGenericRepositoryAsync<T>
    {
        void Delete(object id);
        T GetValue(object id);
        IList<T> GetAll();
        T Save(T entity);
        T Update(T entity);
    }
    public interface IGenericRepositoryAsync<T>
    {
        Task DeleteAsync(object id);
        Task<T> GetValueAsync(object id);
        Task<IList<T>> GetAllAsync();
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
