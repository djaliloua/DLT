using PurchaseManagement.MVVM.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class ProductRepository : IGenericRepository<Product>
    {
        public List<Product> Items { get; private set; }
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public ProductRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                ReferenceHandler=ReferenceHandler.IgnoreCycles
            };
        }
        public async Task Delete(int id)
        {
            Uri uri = new Uri(Constants.GetRestUrl(null, $"{id}"));
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

        public async Task<Product> Get(int id)
        {
            Product product = null;
            Uri uri = new Uri(Constants.GetRestUrl(id.ToString(), "Products"));
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                product = JsonSerializer.Deserialize<Product>(responseString, _serializerOptions);

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            Items = new List<Product>();
            Uri uri = new Uri(Constants.GetRestUrl(null, "Products"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Product>>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }

        public bool ItemExist(int id)
        {
            return Items.Any(x => x.Id == id);
        }

        public async Task<Product> SaveOrUpdateItem(Product item, bool isNewItem = false)
        {
            Uri uri;
            Product product = null;
            try
            {
                string json = JsonSerializer.Serialize(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    uri = new Uri(Constants.GetRestUrl(null, "Products"));
                    response = await _client.PostAsync(uri, content);
                }
                else
                {
                    uri = new Uri(Constants.GetRestUrl(null, $"{item.Id}"));
                    response = await _client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<Product>(responseString, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return product;
        }

        public Task<Product> GetByDate(string date)
        {
            throw new NotImplementedException();
        }
    }
}
