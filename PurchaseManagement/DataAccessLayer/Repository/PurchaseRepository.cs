using PurchaseManagement.MVVM.Models;
using System;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class PurchaseRepository : IGenericRepository<Purchase>
    {
        public IEnumerable<Purchase> Items { get; private set; }
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public PurchaseRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
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

        public async Task<Purchase> Get(int id)
        {
            Purchase purchase = null;
            Uri uri = new Uri(Constants.GetRestUrl(id.ToString(), "Purchases"));
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                purchase = JsonSerializer.Deserialize<Purchase>(responseString, _serializerOptions);

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return purchase;
        }
        public async Task<Purchase> GetByDate(string date)
        {
            Purchase purchase = null;
            Uri uri = new Uri(Constants.GetRestUrl($"date/{date}", "Purchases"));
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                purchase = JsonSerializer.Deserialize<Purchase>(responseString, _serializerOptions);

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetAll()
        {
            Items = new List<Purchase>();
            Uri uri = new Uri(Constants.GetRestUrl(null, "Purchases"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<IEnumerable<Purchase>>(content, _serializerOptions);
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


        public async Task<Purchase> SaveOrUpdateItem(Purchase item, bool isNewItem = false)
        {
            Uri uri;
            Purchase purchase = null;
            try
            {
                string json = JsonSerializer.Serialize(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    uri = new Uri(Constants.GetRestUrl(null, "Purchases"));
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
                    purchase = JsonSerializer.Deserialize<Purchase>(responseString, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return purchase;
        }
    }
}
