using SQLite;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Abstractions;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using SQLiteNetExtensions.Extensions;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IGenericRepository<ProductStatistics> _statisticsRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<MarketModels.Location> _locationRepository;
        private readonly IGenericRepository<Purchase> _purchaseRepository;
        public PurchaseRepository(IGenericRepository<ProductStatistics> statisticsRepository, 
            IProductRepository productRepository,
            IGenericRepository<MarketModels.Location> locationRepository,
            IGenericRepository<Purchase> purchaseRepository)
        {
            _statisticsRepository = statisticsRepository;
            _productRepository = productRepository;
            _locationRepository = locationRepository;
            _purchaseRepository = purchaseRepository;
        }
        public async Task DeleteItem(Purchase item)
        {
            await _purchaseRepository.DeleteItem(item);
            return ;
        }

        public async Task<IEnumerable<Purchase>> GetAllItems()
        {
            string sqlComd = $"select *\r\nfrom Purchases P\r\nOrder by P.PurchaseDate desc\r\n;";
            List<Purchase> purchases = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Purchase>();
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
            string sqlCmd = "select *\r\nfrom Purchases P\r\nwhere P.PurchaseDate= ?;";
            Purchase purchase = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Purchase>();
                purchase = connection.Query<Purchase>(sqlCmd, $"{date:yyyy-MM-dd}").FirstOrDefault();
                
                if (purchase != null)
                    await purchase.LoadPurchaseStatistics(_statisticsRepository);
            }
            return purchase;
        }

        public async Task<Purchase> GetFullPurchaseByDate(DateTime date)
        {
            string sqlCmd = "select *\r\nfrom Purchases P\r\nwhere P.PurchaseDate= ?";
            Purchase purchase = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Purchase>();
                purchase = connection.Query<Purchase>(sqlCmd, $"{date:yyyy-MM-dd}").FirstOrDefault();
                if (purchase != null)
                {
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
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Purchase>();
                purchase = connection.GetWithChildren<Purchase>(id);
            }
            return purchase;
        }

        public async Task<Purchase> SaveOrUpdateItem(Purchase item)
        {
            return await _purchaseRepository.SaveOrUpdateItem(item);
        }
    }
}
