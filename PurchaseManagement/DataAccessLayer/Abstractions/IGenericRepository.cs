namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IGenericRepository<TItem>:IGenericRepositoryAsync<TItem> where TItem : class
    {
        TItem GetItemById(int id);
        List<TItem> GetAllItems();
        TItem SaveOrUpdateItem(TItem item);
        void DeleteItem(TItem item);
    }
    public interface IGenericRepositoryAsync<TItem> where TItem : class
    {
        Task<TItem> GetItemByIdAsync(int id);
        Task<IEnumerable<TItem>> GetAllItemsAsync();
        Task<TItem> SaveOrUpdateItemAsync(TItem item);
        Task DeleteItemAsync(TItem item);
       
    }
}
