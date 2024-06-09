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
                connection.EnableWriteAheadLogging();
                purchases = connection.Query<Purchase>(sqlComd);
                for (int i = 0; i < purchases.Count; i++)
                {
                    await purchases[i].LoadPurchaseStatistics(_statisticsRepository);
                    await purchases[i].LoadProducts(_productRepository, _locationRepository);
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
                    await purchase.LoadPurchaseStatistics(_statisticsRepository);
                }
            }
            return purchase;
        }

        public async Task<Purchase> GetFullPurchaseByDate(DateTime date)
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
                    await purchase.LoadPurchaseStatistics(_statisticsRepository);
                    await purchase.LoadProducts(_productRepository, _locationRepository);
                }
            }
            return purchase;
        }

        public async Task<Purchase> GetItemById(int id)
        {
            await Task.Delay(1);
            Purchase purchase;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Purchase>();
                connection.EnableWriteAheadLogging();
                purchase = connection.Table<Purchase>().FirstOrDefault(p => p.Purchase_Id == id);
            }
            return purchase;
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
