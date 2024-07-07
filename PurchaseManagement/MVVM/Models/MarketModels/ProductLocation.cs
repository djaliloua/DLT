namespace PurchaseManagement.MVVM.Models.MarketModels
{
    public class ProductLocation:BaseEntity
    {
        public double? Course { get; set; }
        public double? Speed { get; set; }
        public bool ReducedAccuracy { get; set; }
        public double? VerticalAccuracy { get; set; }
        public double? Accuracy { get; set; }
        public double? Altitude { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public AltitudeReferenceSystem AltitudeReferenceSystem { get; set; }
        public bool IsFromMockProvider { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
