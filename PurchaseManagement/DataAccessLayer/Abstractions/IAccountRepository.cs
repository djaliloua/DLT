using PurchaseManagement.MVVM.Models.Accounts;


namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IAccountRepository: IGenericRepository<Account>
    {
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();
    }
}
