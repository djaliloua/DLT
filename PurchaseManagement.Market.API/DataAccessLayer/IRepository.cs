namespace PurchaseManagement.Market.API.DataAccessLayer
{
    public interface IRepository<TItem> where TItem : class
    {
        Task<IEnumerable<TItem>> GetAll();
        Task<TItem> Get(int id);
        Task Put(int id, TItem item);
        Task<TItem> Post(TItem item);
        bool ItemExist(int id);
        Task Delete(int id);
    }
}
