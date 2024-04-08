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
        [OneToMany(nameof(Purchase_Id))]
        public IList<Purchase_Items> Purchase_Items { get; set; }
        public Purchases(string title)
        {
            Title = title;
            Purchase_Date = DateTime.Now.ToString("yyy-MM-dd");
        }
        public Purchases()
        {
            
        }
    }
}
