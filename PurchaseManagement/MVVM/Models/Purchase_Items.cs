using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models
{
    [Table("Purchase_Items")]
    public class Purchase_Items
    {
        [PrimaryKey, AutoIncrement]
        public int Item_Id { get; set; }
        [ForeignKey(typeof(Purchases))]
        public int Purchase_Id { get; set; }
        public string Item_Name { get; set; }
        public int Item_Price { get; set; }
        public int Item_Quantity { get; set;}
        
        [ManyToOne]
        public Purchases Purchase { get; set; }
    }
}
