namespace PurchaseManagement.MVVM.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Item_Name { get; set; }
        public double Item_Price { get; set; }
        public double Item_Quantity { get; set; }
        public string Item_Description { get; set; }
        public bool IsPurchased { get; set; }
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public Location Location { get; set; }
        public Product(string item_name, long item_price, long item_quantity, string item_desc)
        {
            Item_Name = item_name;
            Item_Price = item_price;
            Item_Quantity = item_quantity;
            Item_Description = item_desc;
        }
        public Product()
        {

        }
    }
}
