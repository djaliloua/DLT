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
        public long TotalPrice { get; set; }
        public long TotalQuantity { get; set; }
        public PurchaseStatistics(int p_id, int p_count, long total_price, long total_quantity)
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
