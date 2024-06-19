using PurchaseManagement.MVVM.Models.MarketModels;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<IList<Product>> GetAllItemById(int id);
    }
    public class ProductRepository : IProductRepository
    {
        
        public async Task<IList<Product>> GetAllItemById(int id)
        {
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Item_id desc;";
            List<Product> purchase_items = null;
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Product>();
                purchase_items = connection.Query<Product>(sqlCmd, id);
            }
            return purchase_items;
        }
        public async Task DeleteItem(Product item)
        {
            await Task.Delay(1);
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Product>();
                connection.Delete(item);
            }
        }

        public async Task<IEnumerable<Product>> GetAllItems()
        {
            await Task.Delay(1);
            List<Product> products;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Product>();
                products = connection.Table<Product>().ToList();
            }
            return products;
        }

        public async Task<Product> GetItemById(int id)
        {
            await Task.Delay(1);
            Product product;
            using (SQLiteConnection connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Product>();
                product = connection.Table<Product>().FirstOrDefault(p => p.PurchaseId == id);
            }
            return product;
        }

        public async Task<Product> SaveOrUpdateItem(Product item)
        {
            int res = 0;
            await Task.Delay(1);
            using (var connection = new SQLiteConnection(ConstantPath.DatabasePurchase, ConstantPath.Flags))
            {
                connection.CreateTable<Product>();
                if (item.Item_Id != 0)
                    res = connection.Update(item);
                else
                    res = connection.Insert(item);
            }
            return item;
        }
    }
}
