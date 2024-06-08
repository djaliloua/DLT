using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Location = PurchaseManagement.MVVM.Models.Location;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class LocationRepository:IGenericRepository<Location>
    {
        public List<Location> Items { get; private set; }
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;
        public LocationRepository()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
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

        public async Task<Location> Get(int id)
        {
            Location location = null;
            Uri uri = new Uri(Constants.GetRestUrl(id.ToString(), "Products"));
            HttpResponseMessage response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                location = JsonSerializer.Deserialize<Location>(responseString, _serializerOptions);

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return location;
        }

        public async Task<IEnumerable<Location>> GetAll()
        {
            Items = new List<Location>();
            Uri uri = new Uri(Constants.GetRestUrl(null, "Locations"));
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Location>>(content, _serializerOptions);
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

        public async Task<Location> SaveOrUpdateItem(Location item, bool isNewItem = false)
        {
            Location location = null;
            Uri uri;
            try
            {
                string json = JsonSerializer.Serialize(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    uri = new Uri(Constants.GetRestUrl(null, "Locations"));
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
                    location = JsonSerializer.Deserialize<Location>(responseString, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return location;
        }

        public Task<Location> GetByDate(string date)
        {
            throw new NotImplementedException();
        }
    }
}
