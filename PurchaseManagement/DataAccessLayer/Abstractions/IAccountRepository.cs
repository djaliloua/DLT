using Models.Account;
using Repository.Interface;


namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IAccountRepository: IGenericRepository<Account>
    {
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();
    }
}
