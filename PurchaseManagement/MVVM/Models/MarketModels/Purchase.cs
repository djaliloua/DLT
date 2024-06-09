using PurchaseManagement.DataAccessLayer.Repository;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models.MarketModels
{
    [Table("Purchases")]
    public class Purchase
    {
        [PrimaryKey, AutoIncrement]
        public int Purchase_Id { get; set; }
        public string Title { get; set; }
        public string PurchaseDate { get; set; }
        private IList<Product> _products;
        [OneToMany(nameof(Purchase_Id))]
        public IList<Product> Products
        {
            get => _products;
            set => _products = value;
        }

        [ForeignKey(typeof(PurchaseStatistics))]
        public int Purchase_Stats_Id { get; set; }
        [OneToOne]
        public PurchaseStatistics PurchaseStatistics { get; set; }
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            PurchaseDate = dt.ToString("yyyy-MM-dd");
        }
        public Purchase()
        {

        }
        public async Task LoadPurchaseStatistics(IGenericRepository<PurchaseStatistics> repository)
        {
            if (PurchaseStatistics == null)
            {
                PurchaseStatistics = await repository.GetItemById(Purchase_Id);
            }
        }
        public async Task LoadProducts(IProductRepository productRepository, IGenericRepository<MarketModels.Location> locationRepository)
        {
            if (Products == null)
            {
                Products ??= new List<Product>();
                foreach (var item in await productRepository.GetAllItemById(Purchase_Id))
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
