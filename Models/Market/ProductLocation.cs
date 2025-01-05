namespace Models.Market
{
    public class ProductLocation : BaseEntity
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
    public enum AltitudeReferenceSystem
    {
        //
        // Summary:
        //     The altitude reference system was not specified.
        Unspecified,
        //
        // Summary:
        //     The altitude reference system is based on distance above terrain or ground level
        Terrain,
        //
        // Summary:
        //     The altitude reference system is based on an ellipsoid (usually WGS84), which
        //     is a mathematical approximation of the shape of the Earth.
        Ellipsoid,
        //
        // Summary:
        //     The altitude reference system is based on the distance above sea level (parametrized
        //     by a so-called Geoid).
        Geoid,
        //
        // Summary:
        //     The altitude reference system is based on the distance above the tallest surface
        //     structures, such as buildings, trees, roads, etc., above terrain or ground level.
        Surface
    }
}
