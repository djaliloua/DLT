using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models
{
    [Table("Purchases")]
    public class Purchases
    {
        [PrimaryKey, AutoIncrement]
        public int Purchase_Id { get; set; }
        public string Title { get; set; }
        public string Purchase_Date { get; set; }
        
        private IList<Purchase_Items> _purchase_Items = new List<Purchase_Items>();
        [OneToMany(nameof(Purchase_Id))]
        public IList<Purchase_Items> Purchase_Items
        {
            get => _purchase_Items;
            set => _purchase_Items = value; 
        }

        [ForeignKey(typeof(PurchaseStatistics))]
        public int Purchase_Stats_Id { get; set; }
        [OneToOne]
        public PurchaseStatistics PurchaseStatistics { get; set; }
        public Purchases(string title, DateTime dt)
        {
            Title = title;
            Purchase_Date = dt.ToString("yyyy-MM-dd");
        }
        public Purchases()
        {
            
        }
        public Purchases Clone() => MemberwiseClone() as Purchases;
    }
}
