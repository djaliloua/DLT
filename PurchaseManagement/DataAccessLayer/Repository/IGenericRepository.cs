namespace PurchaseManagement.DataAccessLayer.Repository
{
    public interface IGenericRepository<TItem> where TItem : class
    {
        Task<TItem> GetItemById(int id);
        Task<IEnumerable<TItem>> GetAllItems();
        Task<TItem> SaveOrUpdateItem(TItem item);
        Task DeleteItem(TItem item);
    }
}
