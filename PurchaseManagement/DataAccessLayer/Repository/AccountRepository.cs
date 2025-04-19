using Microsoft.EntityFrameworkCore;
using Models.Account;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.ViewModel;
using Repository.Implementation;


namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class AccountRepository : GenericRepositoryViewModel<Account, AccountViewModel>, IAccountRepository
    {
        public AccountRepository()
        {
           
        }
        public async Task<IList<Account>> GetAllItemsAsync()
        {
            return await _table.FromSql($"select *\r\nfrom Accounts acc\r\norder by acc.DateTime desc;").AsNoTracking().ToListAsync();
        }
        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            string sql = "select AC.DateTime, avg(AC.Money) AvgMoney, sum(AC.Money) TotalMoney, count(AC.Money) CountMoney\r\nfrom Accounts AC\r\ngroup by AC.Day\r\nOrder by AC.Day desc\r\n;";
            return await _dbContext.Database.SqlQueryRaw<Statistics>(sql).AsNoTracking().ToListAsync();
        }
        public async Task<IList<MaxMin>> GetMinAsync()
        {
            string sql = "select AC.DateTime, min(AC.Money) Value\r\nfrom Accounts AC;";
            return await _dbContext.Database.SqlQueryRaw<MaxMin>(sql).AsNoTracking().ToListAsync();
        }
        public async Task<IList<MaxMin>> GetMaxAsync()
        {
            string sql = "select AC.DateTime, max(AC.Money) Value\r\nfrom Accounts AC;";
            return await _dbContext.Database.SqlQueryRaw<MaxMin>(sql).AsNoTracking().ToListAsync();
        }

        
    }
}
