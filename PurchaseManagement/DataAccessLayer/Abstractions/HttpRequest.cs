using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public class HttpRequest
    {
        readonly JsonSerializerOptions _serializerOptions;
        readonly HttpClient _httpClient;
        public HttpRequest()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }
        public async Task<bool> DeleteAsync(Uri url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        public async Task<T> Deserialize<T>(Uri uri)
        {
            T item = default;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    item = JsonSerializer.Deserialize<T>(content, _serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return item;
        }
        public async Task<T> Serialize<T>(T item, Uri uri)
        {
            T res = default;
            try
            {
                string json = JsonSerializer.Serialize<T>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await _httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string cont = await response.Content.ReadAsStringAsync();
                    res = JsonSerializer.Deserialize<T>(cont, _serializerOptions);
                }
            }
            catch
            {

            }
            return res;
        }
    }
}
