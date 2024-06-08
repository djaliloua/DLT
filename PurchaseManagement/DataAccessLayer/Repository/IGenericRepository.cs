namespace PurchaseManagement.DataAccessLayer.Repository
{
    public interface IGenericRepository<TItem> where TItem : class
    {
        Task<IEnumerable<TItem>> GetAll();
        Task<TItem> Get(int id);
        Task<TItem> SaveOrUpdateItem(TItem item, bool isNewItem = false);
        bool ItemExist(int id);
        Task Delete(int id);
        Task<TItem> GetByDate(string date);
    }

}
