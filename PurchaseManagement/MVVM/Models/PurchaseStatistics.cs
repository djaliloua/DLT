using SQLite;

namespace PurchaseManagement.MVVM.Models
{
    [Table("PurchaseStatistics")]
    public class PurchaseStatistics
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Purchase_Id { get; set; }
        public string PurchaseCount { get; set; }
        public string TotalPrice { get; set; }
        public string TotalQuantity { get; set; }
        public PurchaseStatistics(int p_id, string p_count, string total_price, string total_quantity)
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
