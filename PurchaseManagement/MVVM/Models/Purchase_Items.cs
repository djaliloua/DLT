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
        public long Item_Price { get; set; }
        public long Item_Quantity { get; set;}
        public string Item_Description { get; set; }
        public bool IsPurchased { get; set; }

        [ForeignKey(typeof(MarketLocation))]
        public int Location_Id { get; set; }
        [OneToOne]
        public MarketLocation Location { get; set; }
        [ManyToOne]
        public Purchases Purchase { get; set; }
        public Purchase_Items(int p_id, string item_name, long item_price, long item_quantity, string item_desc)
        {
            Purchase_Id = p_id;
            Item_Name = item_name;
            Item_Price = item_price;
            Item_Quantity = item_quantity;
            Item_Description = item_desc;
        }
        public bool IsLocation => Location != null;
        public Purchase_Items()
        {
            
        }
    }
}
