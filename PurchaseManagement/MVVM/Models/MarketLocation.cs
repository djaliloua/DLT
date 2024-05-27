using SQLite;
using Microsoft.Maui.Devices.Sensors;

namespace PurchaseManagement.MVVM.Models
{
    [Table("Location")]
    public class MarketLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Location_Id { get; set; }
        public int Purchase_Item_Id { get; set; }
        public int Purchase_Id { get; set; }
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
    }
}
