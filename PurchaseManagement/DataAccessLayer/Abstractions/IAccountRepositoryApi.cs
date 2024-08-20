using PurchaseManagement.MVVM.Models.Accounts;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IAccountRepositoryApi
    {
        Task Delete(int id);
        Task<Account> GetItemById(int id);
        Task<IList<Account>> GetAllItems();
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();
        Task<Account> SaveOrUpdate(Account account);
    }
}
