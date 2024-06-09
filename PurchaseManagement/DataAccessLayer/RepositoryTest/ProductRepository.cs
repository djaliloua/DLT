using PurchaseManagement.MVVM.Models;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.RepositoryTest
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<IList<Product>> GetAllItemById(int id);
    }
    public class ProductRepository : IProductRepository
    {
        //private readonly IGenericRepository<PurchaseStatistics> _statisticsRepository;
        //private readonly IGenericRepository<MVVM.Models.Location> _locationRepository;
        //public ProductRepository(IGenericRepository<PurchaseStatistics> statisticsRepository, 
        //    IGenericRepository<MVVM.Models.Location> locationRepository)
        //{
        //    _statisticsRepository = statisticsRepository;
        //    _locationRepository = locationRepository;
        //}
        public async Task<IList<Product>> GetAllItemById(int id)
        {
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = {id}\r\norder by P.Item_id desc;";
            List<Product> purchase_items = null;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                purchase_items = connection.Query<Product>(sqlCmd);

                //purchase_items = connection.Table<Purchase_Items>().Where(p => p.Purchase_Id == purchaseId).OrderByDescending(x => x.Item_Id).ToList();
            }
            return purchase_items;
        }
        public async Task DeleteItem(Product item)
        {
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<Product>> GetAllItems()
        {
            await Task.Delay(1);
            List<Product> products;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                products = connection.Table<Product>().ToList();
            }
            return products;
        }

        public async Task<Product> GetItemById(int id)
        {
            await Task.Delay(1);
            Product product;
            using (SQLiteConnection connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                product = connection.Table<Product>().FirstOrDefault(p => p.PurchaseId == id);
            }
            return product;
        }

        public async Task<Product> SaveOrUpdateItem(Product item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(Constants.DatabasePurchase, Constants.Flags))
            {
                connection.CreateTable<Product>();
                connection.EnableWriteAheadLogging();
                if (item.Item_Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
