using AutoMapper;
using MVVM;
using CommunityToolkit.Mvvm.Messaging;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class ProductDto : BaseViewModel
    {
        public int Id { get; set; }
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
            set => UpdateObservable(ref _isPurchased, value, () =>
            {
                WeakReferenceMessenger.Default.Send(this, "update");
            });
        }
        private int _purchaseId;
        public int PurchaseId
        {
            get => _purchaseId;
            set => UpdateObservable(ref _purchaseId, value);
        }

        private PurchaseDto _purchase;
        public PurchaseDto Purchase
        {
            get => _purchase;
            set => UpdateObservable(ref _purchase, value);
        }
        public int Counter { get; set; }
        private Mapper mapper;
        public ProductDto(int counter)
        {
            Counter = counter;
            mapper = MapperConfig.InitializeAutomapper();
        }
       
        public ProductDto()
        {
            Counter = 0;
            Item_Name = "Mangue";
            Item_Price = 24;
            Item_Quantity = 10;
            Item_Description = "Je t'aime";
            mapper = MapperConfig.InitializeAutomapper();
        }

    }
}
