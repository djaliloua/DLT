using PurchaseManagement.MVVM.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace PurchaseManagement.DataAccessLayer
{
    public interface IRepositoryAPI
    {
        Task<IList<Purchase>> GetAllItemsAsync();
    }
    public class RepositoryAPI : IRepositoryAPI
    {
        public List<Purchase> Items { get; private set; }
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public RepositoryAPI()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<IList<Purchase>> GetAllItemsAsync()
        {
            Items = new List<Purchase>();

            Uri uri = new Uri(Constants.GetRestUrl(null, "Purchases"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Purchase>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }
    }

    
}
