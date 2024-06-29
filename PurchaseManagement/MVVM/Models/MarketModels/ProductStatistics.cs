using SQLite;

namespace PurchaseManagement.MVVM.Models.MarketModels
{
    [Table("ProductStatistics")]
    public class ProductStatistics:BaseEntity
    {
        public int Purchase_Id { get; set; }
        public int PurchaseCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public ProductStatistics(int p_id, int p_count, double total_price, double total_quantity)
        {
            Purchase_Id = p_id;
            PurchaseCount = p_count;
            TotalPrice = total_price;
            TotalQuantity = total_quantity;
        }
        public ProductStatistics()
        {

        }
    }
}
