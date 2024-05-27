using AutoMapper;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.ServiceLocator;
using Microsoft.Maui.ApplicationModel;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class Purchase_ItemsDTO : BaseViewModel
    {
        public int Item_Id { get; set; }
        public int Purchase_Id { get; set; }
        private string item_name = "Kello";
        public string Item_Name
        {
            get => item_name;
            set => UpdateObservable(ref item_name, value);
        }
        private long item_price = 1000;
        public long Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }
        private long item_quantity = 10;
        public long Item_Quantity
        {
            get => item_quantity;
            set => UpdateObservable(ref item_quantity, value);
        }
        private string _item_desc = "Je t'aime bien";
        public string Item_Description
        {
            get => _item_desc;
            set => UpdateObservable(ref _item_desc, value);
        }
        private bool _isPurchased;
        public bool IsPurchased
        {
            get => _isPurchased;
            set => UpdateObservable(ref _isPurchased, value, async () =>
            {
                //PurchaseStatistics purchaseStatistics;
                var db = new Repository();
                var mapper = MapperConfig.InitializeAutomapper();
                Purchases purchases = await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
                if (purchases != null)
                {
                    //Purchase = mapper.Map<PurchasesDTO>(purchases);
                    await db.SavePurchaseItemAsync(mapper.Map<Purchase_Items>(this));
                }
            });
        }
        public int Counter { get; set; }
        private Mapper mapper;
        public Purchase_ItemsDTO(int counter)
        {
            Counter = counter;
            mapper = MapperConfig.InitializeAutomapper();
        }
        private MarketLocationDTO _location;
        public MarketLocationDTO Location
        {
            get => _location;
            set => UpdateObservable(ref _location, value);
        }
        private PurchasesDTO _purchases;
        public PurchasesDTO Purchase
        {
            get => _purchases;
            set => UpdateObservable(ref _purchases, value);
        }
        //public bool IsLocation => Location != null; 
        private bool isLocation;
        public bool IsLocation
        {
            get => isLocation;
            set => UpdateObservable(ref isLocation, value);
        }
        public Purchase_ItemsDTO()
        {
            Counter = 0;
        }

    }
}
