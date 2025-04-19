namespace Models.Market
{
    public class ProductStatistics : BaseEntity
    {
        public int PurchaseCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
        public ProductStatistics(int p_count, double total_price, double total_quantity)
        {
            PurchaseCount = p_count;
            TotalPrice = total_price;
            TotalQuantity = total_quantity;
        }
        public ProductStatistics()
        {

        }
    }
}
