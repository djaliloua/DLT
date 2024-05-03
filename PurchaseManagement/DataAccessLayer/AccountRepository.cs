using PurchaseManagement.MVVM.Models;
using SQLite;

namespace PurchaseManagement.DataAccessLayer
{
    public class Statistics
    {
        public string Day { get; set; }
        public long TotalMoney { get; set; }
        public long AvgMoney { get; set; }
        public int CountMoney { get; set; }
    }
    public class MaxMin
    {
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public MaxMin(DateTime _dt, double val)
        {
            Value = val;
            DateTime = _dt;
        }
        public MaxMin()
        {
            Value = 0;
        }
    }
    public class AccountRepository : IAccountRepository
    {
        public async Task<IList<Account>> GetAllAsync()
        {
            await Task.Delay(1);
            IList<Account> accounts = new List<Account>();
            using(var conn = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                conn.CreateTable<Account>();
                accounts = conn.Table<Account>().OrderByDescending(a => a.DateTime).ToList();
            }
            return accounts;
        }

        public async Task<int> SaveOrUpdateAsync(Account account)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                if (account.Id != 0)
                    res = connection.Update(account);
                else
                    res = connection.Insert(account);
            }
            return res;
        }
        public async Task<int> DeleteAsync(Account account)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Account>();
                res = connection.Delete(account);
            }
            return res;
        }
        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            string sql = "select AC.Day, avg(AC.Money) AvgMoney, sum(AC.Money) TotalMoney, count(AC.Money) CountMoney\r\nfrom Account AC\r\ngroup by AC.Day\r\nOrder by AC.Day desc\r\n;";
            await Task.Delay(1);
            IList <Statistics> statistics = new List<Statistics>();
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
    }
    public interface IAccountRepository
    {
        Task<IList<Account>> GetAllAsync();
        Task<int> SaveOrUpdateAsync(Account account);
        Task<int> DeleteAsync(Account account);
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();

    }
}
