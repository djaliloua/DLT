namespace PurchaseManagement.Market.API.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Purchase_Date { get; set; }
        public ICollection<Product> Products { get; } = new List<Product>();
        public Purchase(string title, DateTime dt)
        {
            Title = title;
            Purchase_Date = dt.ToString("yyyy-MM-dd");
        }
        public Purchase()
        {

        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Item_Name { get; set; }
        public long Item_Price { get; set; }
        public long Item_Quantity { get; set; }
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
    public class Location
    {
        public int Id { get; set; }
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
        public Product Product { get; set; }
    }
    public enum AltitudeReferenceSystem
    {
        /// <summary>The altitude reference system was not specified.</summary>
        Unspecified = 0,

        /// <summary>The altitude reference system is based on distance above terrain or ground level</summary>
        Terrain = 1,

        /// <summary>The altitude reference system is based on an ellipsoid (usually WGS84), which is a mathematical approximation of the shape of the Earth.</summary>
        Ellipsoid = 2,

        /// <summary>The altitude reference system is based on the distance above sea level (parametrized by a so-called Geoid).</summary>
        Geoid = 3,

        /// <summary>The altitude reference system is based on the distance above the tallest surface structures, such as buildings, trees, roads, etc., above terrain or ground level.</summary>
        Surface = 4
    }
    public class ProductStatistics
    {
        public int Id { get; set; }
        public int ProductCount { get; set; }
        public double TotalProductsPrice { get; set; }
        public double TotalProductsQuantity { get; set; }
        //
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public ProductStatistics(int p_id, int p_count, double total_price, double total_quantity)
        {
            PurchaseId = p_id;
            ProductCount = p_count;
            TotalProductsPrice = total_price;
            TotalProductsQuantity = total_quantity;
        }
        public ProductStatistics()
        {

        }
    }
}
