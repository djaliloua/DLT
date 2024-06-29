using PurchaseManagement.DataAccessLayer.Abstractions;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models.MarketModels
{
    [Table("Purchases")]
    public class Purchase:BaseEntity
    {
        public string Title { get; set; }
        public string PurchaseDate { get; set; }
        private IList<Product> _products;
        [OneToMany(nameof(Id))]
        public IList<Product> Products
        {
            get => _products;
            set => _products = value;
        }

        [ForeignKey(typeof(ProductStatistics))]
        public int ProductStatId { get; set; }
        [OneToOne]
        public ProductStatistics PurchaseStatistics { get; set; }
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            PurchaseDate = dt.ToString("yyyy-MM-dd");
        }
        public Purchase()
        {

        }
        public async Task LoadPurchaseStatistics(IGenericRepository<ProductStatistics> repository)
        {
            if (PurchaseStatistics == null)
            {
                PurchaseStatistics = await repository.GetItemById(Id);
            }
        }
        public async Task LoadProducts(IProductRepository productRepository, IGenericRepository<MarketModels.Location> locationRepository)
        {
            if (Products == null)
            {
                Products ??= new List<Product>();
                foreach (var item in await productRepository.GetAllItemById(Id))
                {
                    item.Purchase = this;
                    await item.LoadLoacation(locationRepository);
                    Products.Add(item);
                }
            }
        }
        public Purchase Clone() => MemberwiseClone() as Purchase;
    }
}
