using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.Accounts;
using SQLite;


namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IGenericRepository<Account> _accountRepository;
        public AccountRepository(IGenericRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task DeleteItem(Account item)
        {
            await _accountRepository.DeleteItem(item);
        }

        public async Task<IEnumerable<Account>> GetAllItems()
        {
            await Task.Delay(1);
            string sqlCmd = "select *\r\nfrom account acc\r\norder by acc.DateTime desc;";
            IList<Account> accounts = new List<Account>();
            using (var conn = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                conn.CreateTable<Account>();
                accounts = conn.Query<Account>(sqlCmd);
            }
            return accounts;
        }

        public async Task<Account> GetItemById(int id)
        {
            return await _accountRepository.GetItemById(id);
        }

        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            string sql = "select AC.DateTime, avg(AC.Money) AvgMoney, sum(AC.Money) TotalMoney, count(AC.Money) CountMoney\r\nfrom Account AC\r\ngroup by AC.Day\r\nOrder by AC.Day desc\r\n;";
            await Task.Delay(1);
            IList<Statistics> statistics = new List<Statistics>();
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
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
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
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
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Account>();
                statistics = connection.Query<MaxMin>(sql);
            }
            return statistics;
        }

        public async Task<Account> SaveOrUpdateItem(Account item)
        {
            
            return await _accountRepository.SaveOrUpdateItem(item);
        }
    }
}
