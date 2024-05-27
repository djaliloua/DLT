using MVVM;
using Microsoft.Maui.Devices.Sensors;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class MarketLocationDTO:BaseViewModel
    {
        private int _location_id;
        public int Location_Id
        {
            get => _location_id;
            set => UpdateObservable(ref _location_id, value);
        }
        private int _purchase_Item_Id;
        public int Purchase_Item_Id
        {
            get => _purchase_Item_Id;
            set => UpdateObservable(ref _purchase_Item_Id, value);
        }
        private int _purchase_Id;
        public int Purchase_Id
        {
            get => _purchase_Id;
            set => UpdateObservable(ref _purchase_Id, value);
        }
        private double? _course;
        public double? Course
        {
            get => _course;
            set => UpdateObservable(ref _course, value);
        }
        private double? _speed;
        public double? Speed
        {
            get => _speed;
            set => UpdateObservable(ref _speed, value);
        }
        private bool _reducedAccuracy;
        public bool ReducedAccuracy
        {
            get => _reducedAccuracy;
            set => UpdateObservable(ref _reducedAccuracy, value);
        }
        private double? _verticalAccuracy;
        public double? VerticalAccuracy
        {
            get => _verticalAccuracy;
            set => UpdateObservable(ref _verticalAccuracy, value);
        }
        private double? _accuracy;
        public double? Accuracy
        {
            get => _accuracy;
            set => UpdateObservable(ref _accuracy, value);
        }
        private double? _altitude;
        public double? Altitude
        {
            get => _altitude;
            set => UpdateObservable(ref _altitude, value);
        }
        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => UpdateObservable(ref _longitude, value); 
        }
        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => UpdateObservable(ref _latitude, value);
        }
        private AltitudeReferenceSystem _altitudeReferenceSystem;
        public AltitudeReferenceSystem AltitudeReferenceSystem
        {
            get => _altitudeReferenceSystem;
            set => UpdateObservable(ref _altitudeReferenceSystem, value);
        }
        private bool _isFromMockProvider;
        public bool IsFromMockProvider
        {
            get => _isFromMockProvider;
            set => UpdateObservable(ref _isFromMockProvider, value);
        }
    }
}
