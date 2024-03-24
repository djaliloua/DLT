namespace ManagPassWord.Data_AcessLayer
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<int> CountItemAsync();
        T GetById(int id);
        Task<int> DeleteAll();
        Task<int> DeleteById(T item);
        Task<int> SaveItemAsync(T obj);
        Task<int> SaveToCsv();
        Task Init();
    }
}
