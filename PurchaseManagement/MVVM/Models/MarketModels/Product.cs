using PurchaseManagement.DataAccessLayer.Repository;
using SQLite;
using SQLiteNetExtensions.Attributes;



namespace PurchaseManagement.MVVM.Models.MarketModels
{
    [Table("Purchase_Items")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Item_Id { get; set; }
        [ForeignKey(typeof(Purchase))]
        public int PurchaseId { get; set; }
        public string Item_Name { get; set; }
        public long Item_Price { get; set; }
        public long Item_Quantity { get; set; }
        public string Item_Description { get; set; }
        public bool IsPurchased { get; set; }

        [ForeignKey(typeof(Location))]
        public int Location_Id { get; set; }
        [OneToOne]
        public Location Location { get; set; }
        [ManyToOne]
        public Purchase Purchase { get; set; }
        public Product(int p_id, string item_name, long item_price, long item_quantity, string item_desc)
        {
            PurchaseId = p_id;
            Item_Name = item_name;
            Item_Price = item_price;
            Item_Quantity = item_quantity;
            Item_Description = item_desc;
        }
        public bool IsLocation => Location != null;
        public Product()
        {

        }
        public async Task LoadPurchase(IPurchaseRepository purchaseRepository, Purchase purchase)
        {
            if (PurchaseId != 0 && Purchase == null)
            {
                Purchase = await purchaseRepository.GetItemById(PurchaseId);
            }
        }
        public async Task LoadLoacation(IGenericRepository<MarketModels.Location> locationRepository)
        {
            if (Location_Id != 0 && Location == null)
            {
                Location = await locationRepository.GetItemById(Location_Id);
            }
        }
    }
}
