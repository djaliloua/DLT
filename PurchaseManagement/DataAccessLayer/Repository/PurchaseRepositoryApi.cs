using Mapster;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.ExtensionMethods;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class ProcessUrl
    {
        public static string GetRestUrl(object id, string parameter, int port=5156)
        {
            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? $"http://10.0.2.2:{port}" : $"http://localhost:{port}";
            string baseUrl;
            if (id == null)
                baseUrl = $"{baseAddress}/api/{parameter}";
            else
                baseUrl = $"{baseAddress}/api/{parameter}/{id}";

            return baseUrl;
        }
    }
    public class PurchaseRepositoryApi : IPurchaseRepositoryApi
    {
        public List<Purchase> Items { get; private set; }
        private HttpRequest _httpRequest;
        public PurchaseRepositoryApi()
        {
            _httpRequest = new HttpRequest();
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<PurchaseDto> DeleteProduct(Product product)
        {
            Uri uri = new Uri(ProcessUrl.GetRestUrl($"product/{product.Id}", "Purchases"));
            HttpRequestPurchase httpRequest = new HttpRequestPurchase();
            var res = await httpRequest.Delete(uri);
            return res.Adapt<PurchaseDto>();
        }
        public async Task<List<Purchase>> GetAllItems()
        {
            Items = new List<Purchase>();

            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Purchases"));
            Items = await _httpRequest.Deserialize<List<Purchase>>(uri);
            return Items;
        }
       
        public async Task<Purchase> GetByDate(string dt)
        {
            Purchase purchase = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(dt, "Purchases/date"));
            purchase = await _httpRequest.Deserialize<Purchase>(uri);
            return purchase;
        }

        public async Task<Purchase> GetById(int id)
        {
            Purchase purchase = null;
            Uri uri = new Uri(ProcessUrl.GetRestUrl(id, "Purchases"));
            purchase = await _httpRequest.Deserialize<Purchase>(uri);
            return purchase;
        }

        public async Task<Purchase> SaveOrUpdate(Purchase purchase)
        {
            Uri uri = new Uri(ProcessUrl.GetRestUrl(null, "Purchases"));
            Purchase purch = await _httpRequest.Serialize(purchase, uri);
            return purch;
        }
        public async Task<PurchaseDto> RemoveProduct(Product product)
        {
            await DeleteProduct(product);
            Purchase p = (await GetAllItems()).FirstOrDefault(x => x.Id == product.Purchase.Id);
            return p.ToDto();
        }
    }
}
