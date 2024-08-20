using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.Accounts;
using System.Security.Principal;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class AccountRepositoryApi : IAccountRepositoryApi
    {
        private readonly HttpRequest _httpRequest;
        public List<Account> Items { get; private set; }
        public AccountRepositoryApi()
        {
            _httpRequest = new HttpRequest();
        }
        public async Task Delete(int id)
        {
            Uri uri = new Uri(ProcessUrl.GetRestUrl(id, "Accounts", port: 5116));
            bool IsDeleted = await _httpRequest.DeleteAsync(uri);
        }

        public async Task<IList<Account>> GetAllItems()
        {
            Items = new List<Account>();

            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Accounts", port:5116));
            Items = await _httpRequest.Deserialize<List<Account>>(uri);
            return Items;
        }

        public async Task<Account> GetItemById(int id)
        {
            Account purchase = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(id, "Accounts", port: 5116));
            purchase = await _httpRequest.Deserialize<Account>(uri);
            return purchase;
        }

        public async Task<IList<MaxMin>> GetMaxAsync()
        {
            List<MaxMin> maxValue = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Accounts/Max", port: 5116));
            maxValue = await _httpRequest.Deserialize<List<MaxMin>>(uri);
            return maxValue;
        }

        public async Task<IList<MaxMin>> GetMinAsync()
        {
            List<MaxMin> minValue = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Accounts/Min", port: 5116));
            minValue = await _httpRequest.Deserialize<List<MaxMin>>(uri);
            return minValue;
        }

        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            List<Statistics> statistics = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Accounts/Statistics", port: 5116));
            statistics = await _httpRequest.Deserialize<List<Statistics>>(uri);
            return statistics;
        }

        public async Task<Account> SaveOrUpdate(Account account)
        {
            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Accounts", port: 5116));
            Account acc = await _httpRequest.Serialize(account, uri);
            return acc;
        }
    }
}
