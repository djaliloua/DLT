using PurchaseManagement.MVVM.Models;
using SQLite;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

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

        public async Task<Account> SaveOrUpdateAsync(Account account)
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
            return account;
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
        Task<Account> SaveOrUpdateAsync(Account account);
        Task<int> DeleteAsync(Account account);
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();

    }

    public class AccountRepositoryAPI : IAccountRepositoryAPI
    {
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public List<Account> Items { get; private set; }

        public AccountRepositoryAPI()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

        }
        public async Task DeleteAccount(int id)
        {
            Uri uri = new Uri(Constants.GetRestUrl(id.ToString(), "Accounts"));

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tTodoItem successfully deleted.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public Task<Account> GetAccount(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Account>> GetAccounts()
        {
            Items = new List<Account>();

            Uri uri = new Uri(Constants.GetRestUrl(null, "Accounts"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Account>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }

        public Task<IList<MaxMin>> GetMaxAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<MaxMin>> GetMinAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Statistics>> GetStatisticsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Account> PostAccount(Account account)
        {
            Uri uri = new Uri(Constants.GetRestUrl(null, "Accounts"));
            try
            {
                string json = JsonSerializer.Serialize(account, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tConfigModel successfully saved.");
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return account;
        }

        public async Task<Account> PutAccount(int id, Account account)
        {
            Uri uri = new Uri(Constants.GetRestUrl(null, "Accounts"));
            try
            {
                string json = JsonSerializer.Serialize<Account>(account, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await _client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                    Debug.WriteLine(@"\tConfigModel successfully saved.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return account;
        }
        public Task<bool> AccountExists(int id)
        {
            throw new NotImplementedException();
        }
    }
    public interface IAccountRepositoryAPI
    {
        Task<IList<Account>> GetAccounts();
        Task<Account> GetAccount(int id);
        Task DeleteAccount(int id);
        Task<Account> PutAccount(int id, Account account);
        Task<Account> PostAccount(Account account);
        Task<IList<Statistics>> GetStatisticsAsync();
        Task<IList<MaxMin>> GetMaxAsync();
        Task<IList<MaxMin>> GetMinAsync();
        Task<bool> AccountExists(int id);

    }
}
