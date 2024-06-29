using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Abstractions;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class StatisticsRepository : IGenericRepository<ProductStatistics>
    {
        private readonly IProductRepository _productRepository;
        public StatisticsRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task DeleteItem(ProductStatistics item)
        {
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<ProductStatistics>();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<ProductStatistics>> GetAllItems()
        {
            await Task.Delay(1);
            List<ProductStatistics> items;
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<ProductStatistics>();
                items = connection.Table<ProductStatistics>().ToList();
            }
            return items;
        }

        public async Task<ProductStatistics> GetItemById(int id)
        {
            await Task.Delay(1);
            ProductStatistics stat = new();
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<ProductStatistics>();
                stat = connection.Table<ProductStatistics>().FirstOrDefault(s => s.Id == id);
            }
            return stat;
        }
        
        public async Task<ProductStatistics> SaveOrUpdateItem(ProductStatistics item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<ProductStatistics>();
                
                if (item.Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
