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
        public int TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public PurchaseStatistics(int p_id, int p_count, int total_price, int total_quantity)
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
