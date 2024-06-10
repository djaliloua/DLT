using PurchaseManagement.MVVM.Models.Accounts;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public interface IAccountRepository: IGenericRepository<Account>
    {
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();
    }
    public class AccountRepository : IAccountRepository
    {
        public async Task DeleteItem(Account item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                res = connection.Delete(item);
            }
        }

        public async Task<IEnumerable<Account>> GetAllItems()
        {
            await Task.Delay(1);
            string sqlCmd = "select *\r\nfrom account acc\r\norder by acc.Day;";
            IList<Account> accounts = new List<Account>();
            using (var conn = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                conn.CreateTable<Account>();
                accounts = conn.Query<Account>(sqlCmd);
            }
            return accounts;
        }

        public async Task<Account> GetItemById(int id)
        {
            await Task.Delay(1);
            Account account = new();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                connection.EnableWriteAheadLogging();
                account = connection.Table<Account>().FirstOrDefault(a => a.Id == id);
            }
            return account;
        }

        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            string sql = "select AC.DateTime, avg(AC.Money) AvgMoney, sum(AC.Money) TotalMoney, count(AC.Money) CountMoney\r\nfrom Account AC\r\ngroup by AC.Day\r\nOrder by AC.Day desc\r\n;";
            await Task.Delay(1);
            IList<Statistics> statistics = new List<Statistics>();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                statistics = connection.Query<Statistics>(sql);
            }
            return statistics;
        }
        public async Task<IList<MaxMin>> GetMinAsync()
        {
            string sql = "select AC.DateTime, min(AC.Money) Value\r\nfrom Account AC;";
            await Task.Delay(1);
            IList<MaxMin> statistics = new List<MaxMin>();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                statistics = connection.Query<MaxMin>(sql);
            }
            return statistics;
        }
        public async Task<IList<MaxMin>> GetMaxAsync()
        {
            string sql = "select AC.DateTime, max(AC.Money) Value\r\nfrom Account AC;";
            await Task.Delay(1);
            IList<MaxMin> statistics = new List<MaxMin>();
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                statistics = connection.Query<MaxMin>(sql);
            }
            return statistics;
        }

        public async Task<Account> SaveOrUpdateItem(Account item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                if (item.Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
    
    
}
