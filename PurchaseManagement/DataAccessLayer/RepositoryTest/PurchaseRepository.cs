using PurchaseManagement.MVVM.Models;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.RepositoryTest
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        Task<Purchase> GetPurchaseByDate(DateTime date);
        Task<Purchase> GetFullPurchaseByDate(DateTime date);
    }
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IGenericRepository<PurchaseStatistics> _statisticsRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<MVVM.Models.Location> _locationRepository;
        public PurchaseRepository(IGenericRepository<PurchaseStatistics> statisticsRepository, 
            IProductRepository productRepository,
            IGenericRepository<MVVM.Models.Location> locationRepository)
        {
            _statisticsRepository = statisticsRepository;
            _productRepository = productRepository;
            _locationRepository = locationRepository;
        }
        public Task DeleteItem(Purchase item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> GetAllItems()
        {
            string sqlComd = $"select *\r\nfrom Purchases P\r\nOrder by P.PurchaseDate desc\r\n;";
            List<Purchase> purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase>();
                connection.CreateTable<PurchaseStatistics>();
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                purchases = connection.Query<Purchase>(sqlComd);
                //purchases = connection.Table<Purchases>().OrderByDescending(p => p.Purchase_Date).ToList();
                for (int i = 0; i < purchases.Count; i++)
                {
                    purchases[i].PurchaseStatistics = await _statisticsRepository.GetItemById(purchases[i].Purchase_Id);
                    IList<Product> purchase_items = await _productRepository.GetAllItemById(purchases[i].Purchase_Id);
                    foreach (Product purchase_item in purchase_items)
                    {
                        purchase_item.Purchase = purchases[i];
                        purchase_item.Location = await _locationRepository.GetItemById(purchase_item.Location_Id);
                        purchases[i].Products.Add(purchase_item);
                    }
                }
            }
            return purchases;
        }
        public async Task<Purchase> GetPurchaseByDate(DateTime date)
        {
            string sqlCmd = $"select *\r\nfrom Purchases P\r\nwhere P.PurchaseDate= '{date:yyyy-MM-dd}';";
            Purchase purchase = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase>();
                var p = connection.Query<Purchase>(sqlCmd);
                if (p != null && p.Count > 0)
                {
                    purchase = p[0];
                    purchase.PurchaseStatistics = await _statisticsRepository.GetItemById(purchase.Purchase_Id);
                }
            }
            return purchase;
        }

        public async Task<Purchase> GetFullPurchaseByDate(DateTime date)
        {
            string sqlCmd = $"select *\r\nfrom Purchases P\r\nwhere P.PurchaseDate= '{date:yyyy-MM-dd}';";
            Purchase purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase>();
                var p = connection.Query<Purchase>(sqlCmd);
                if (p != null && p.Count > 0)
                {
                    purchases = p[0];
                    purchases.PurchaseStatistics = await _statisticsRepository.GetItemById(purchases.Purchase_Id);
                    var data = await _productRepository.GetAllItemById(purchases.Purchase_Id);

                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].Purchase = purchases;
                        data[i].Location = await _locationRepository.GetItemById(data[i].Location_Id);
                        purchases.Products.Add(data[i]);
                    }

                }
            }
            return purchases;
        }

        public Task<Purchase> GetItemById(int id)
        {
            throw new NotImplementedException();
        }

        

        public async Task<Purchase> SaveOrUpdateItem(Purchase item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase>();
                connection.EnableWriteAheadLogging();
                if (item.Purchase_Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
