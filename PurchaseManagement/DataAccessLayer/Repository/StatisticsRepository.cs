using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class StatisticsRepository : IGenericRepository<PurchaseStatistics>
    {
        private readonly IProductRepository _productRepository;
        public StatisticsRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task DeleteItem(PurchaseStatistics item)
        {
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<PurchaseStatistics>();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<PurchaseStatistics>> GetAllItems()
        {
            await Task.Delay(1);
            List<PurchaseStatistics> items;
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<PurchaseStatistics>();
                items = connection.Table<PurchaseStatistics>().ToList();
            }
            return items;
        }

        public async Task<PurchaseStatistics> GetItemById(int id)
        {
            await Task.Delay(1);
            PurchaseStatistics stat = new();
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<PurchaseStatistics>();
                stat = connection.Table<PurchaseStatistics>().FirstOrDefault(s => s.Id == id);
            }
            return stat;
        }
        private async Task<double> GetTotalValue(int id, string colname)
        {
            var d = await _productRepository.GetAllItemById(id);
            double result = 0;
            if (colname == "Price")
                result = d.Sum(x => x.Item_Price);
            else
                result = d.Sum(x => x.Item_Quantity);
            return result;
        }
        private async Task<int> CountPurchaseItems(int purchase_id)
        {
            IList<Product> items = await _productRepository.GetAllItemById(purchase_id);
            return items.Count();
        }
        public async Task<PurchaseStatistics> SaveOrUpdateItem(PurchaseStatistics item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<PurchaseStatistics>();
                item ??= new();
                item.PurchaseCount = await CountPurchaseItems(item.Purchase_Id);
                item.TotalPrice = await GetTotalValue(item.Purchase_Id, "Price");
                item.TotalQuantity = await GetTotalValue(item.Purchase_Id, "Quantity");
                if (item.Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
