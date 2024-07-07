using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Utilities
{
    public static class StatisticRepoUtility
    {
        private static readonly IProductRepository _productRepository;
        static StatisticRepoUtility()
        {
            _productRepository = ViewModelLocator.GetService<IProductRepository>();
        }
        public static async Task<ProductStatistics> CreateOrUpdatePurchaseStatistics(ProductStatistics item)
        {
            item ??= new();
            //item.PurchaseCount = await CountPurchaseItems(item.Purchase_Id);
            //item.TotalPrice = await GetTotalValue(item.Purchase_Id, "Price");
            //item.TotalQuantity = await GetTotalValue(item.Purchase_Id, "Quantity");
            return item;
        }
        private static async Task<double> GetTotalValue(int id, string colname)
        {
            var d = await _productRepository.GetAllItemByIdAsync(id);
            double result = 0;
            if (colname == "Price")
                result = d.Sum(x => x.Item_Price);
            else
                result = d.Sum(x => x.Item_Quantity);
            return result;
        }
        private static async Task<int> CountPurchaseItems(int purchase_id)
        {
            IList<Product> items = await _productRepository.GetAllItemByIdAsync(purchase_id);
            return items.Count();
        }
    }
}
