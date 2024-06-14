using MVVM;
using CommunityToolkit.Mvvm.Messaging;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class ProductDto : BaseViewModel
    {
        public int Item_Id { get; set; }
        public int PurchaseId { get; set; }
        private string item_name;
        public string Item_Name
        {
            get => item_name;
            set => UpdateObservable(ref item_name, value);
        }
        private long item_price;
        public long Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }
        private long item_quantity;
        public long Item_Quantity
        {
            get => item_quantity;
            set => UpdateObservable(ref item_quantity, value);
        }
        private string _item_desc;
        public string Item_Description
        {
            get => _item_desc;
            set => UpdateObservable(ref _item_desc, value);
        }
        private bool _isPurchased;
        public bool IsPurchased
        {
            get => _isPurchased;
            set => UpdateObservable(ref _isPurchased, value, () =>
            {
                WeakReferenceMessenger.Default.Send(this, "update");
            });
        }
        private int _location_id;
        public int Location_Id
        {
            get => _location_id;
            set => _location_id = value;
        }
        private LocationDto _location;
        public LocationDto Location
        {
            get => _location;
            set => UpdateObservable(ref _location, value, () =>
            {
                if (value != null)
                    IsLocation = true;
            });
        }
        private PurchasesDTO _purchases;
        public PurchasesDTO Purchase
        {
            get => _purchases;
            set => UpdateObservable(ref _purchases, value);
        }
        private bool isLocation;
        public bool IsLocation
        {
            get => isLocation;
            set => UpdateObservable(ref isLocation, value);
        }
        public int Counter { get; set; }

        #region Constructors
        public ProductDto(int counter)
        {
            Counter = counter;
        }
        public ProductDto()
        {
            Counter = 0;
        }
        #endregion
        
        

    }
}
