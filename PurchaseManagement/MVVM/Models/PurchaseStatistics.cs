using SQLite;

namespace PurchaseManagement.MVVM.Models
{
    [Table("PurchaseStatistics")]
    public class PurchaseStatistics
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Purchase_Id { get; set; }
        public int PurchaseCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public PurchaseStatistics(int p_id, int p_count, double total_price, double total_quantity)
        {
            Purchase_Id = p_id;
            PurchaseCount = p_count;
            TotalPrice = total_price;
            TotalQuantity = total_quantity;
        }
        public PurchaseStatistics()
        {
            
        }
    }
}
