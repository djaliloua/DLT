using PurchaseManagement.MVVM.Models.MarketModels;
using SQLite;

namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public class ProductRepository : IProductRepository
    {
        private readonly IGenericRepository<Product> _productRepository;
        public ProductRepository(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IList<Product>> GetAllItemById(int id)
        {
            string sqlCmd = $"select *\r\nfrom Purchase_items P\r\nwhere P.PurchaseId = ?\r\norder by P.Id desc;";
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
            await _productRepository.DeleteItem(item);
        }

        public async Task<IEnumerable<Product>> GetAllItems()
        {
            
            return await _productRepository.GetAllItems();
        }

        public async Task<Product> GetItemById(int id)
        {
           
            return await _productRepository.GetItemById(id);
        }

        public async Task<Product> SaveOrUpdateItem(Product item)
        {
            return await _productRepository.SaveOrUpdateItem(item);
        }
    }
}
