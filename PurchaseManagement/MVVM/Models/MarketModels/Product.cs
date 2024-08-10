namespace PurchaseManagement.MVVM.Models.MarketModels
{
    public class Product:BaseEntity
    {
        public string Item_Name { get; set; }
        public double Item_Price { get; set; }
        public double Item_Quantity { get; set; }
        public string Item_Description { get; set; }
        public bool IsPurchased { get; set; }
        public virtual ProductLocation ProductLocation { get; set; }
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
        public Product(string item_name, double item_price, double item_quantity, string item_desc)
        {
            Item_Name = item_name;
            Item_Price = item_price;
            Item_Quantity = item_quantity;
            Item_Description = item_desc;
        }
        public bool IsLocation => ProductLocation != null;
        public Product()
        {

        }
    }
}
