﻿using MVVM;
using CommunityToolkit.Mvvm.Messaging;

namespace PurchaseManagement.MVVM.Models.ViewModel
{
    public class Factory
    {
        public static ProductViewModel CreateObject(int counter)
        {
            return new ProductViewModel(0);
        }
        public static ProductViewModel CreateObject(ProductStatisticsDto stat)
        {
            return stat == null ? new ProductViewModel(0) : new ProductViewModel(stat.PurchaseCount);
        }
        public static ProductViewModel CreateObject()
        {
            return new ProductViewModel();
        }
    }
    public class ProductViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        private string item_name = "Tiramissu";
        public string Item_Name
        {
            get => item_name;
            set => UpdateObservable(ref item_name, value);
        }
        private double item_price = 1000;
        public double Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }
        private double item_quantity = 10;
        public double Item_Quantity
        {
            get => item_quantity;
            set => UpdateObservable(ref item_quantity, value);
        }
        private string _item_desc = "Ahahah this is a simple test";
        public string Item_Description
        {
            get => _item_desc;
            set => UpdateObservable(ref _item_desc, value);
        }
        private bool _isPurchased;
        public bool IsPurchased
        {
            get => _isPurchased;
            set
            {
                if(_isPurchased != value)
                {
                    _isPurchased = value;
                    WeakReferenceMessenger.Default.Send(this, "update");
                    OnPropertyChanged();
                }
            }
        }
        private int _location_id;
        public int Location_Id
        {
            get => _location_id;
            set => _location_id = value;
        }
        private LocationViewModel _location;
        public LocationViewModel ProductLocation
        {
            get => _location;
            set => UpdateObservable(ref _location, value, () =>
            {
                if (value != null)
                    IsLocation = true;
            });
        }
        private PurchaseViewModel _purchases;
        public PurchaseViewModel Purchase
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
        public ProductViewModel(int counter)
        {
            Counter = counter;
        }
        public ProductViewModel()
        {
            Counter = 0;
        }
        #endregion
        
        public ProductViewModel Clone() => (ProductViewModel)MemberwiseClone();

    }
}
