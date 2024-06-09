using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models
{
    [Table("Purchases")]
    public class Purchase
    {
        [PrimaryKey, AutoIncrement]
        public int Purchase_Id { get; set; }
        public string Title { get; set; }
        public string PurchaseDate { get; set; }
        
        private IList<Product> _products = new List<Product>();
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
        public Purchase Clone() => MemberwiseClone() as Purchase;
    }
}
