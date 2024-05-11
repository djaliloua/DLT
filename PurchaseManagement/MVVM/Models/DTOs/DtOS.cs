using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class Purchase_ItemsDTO : BaseViewModel
    {
        public int Item_Id { get; set; }
        public int Purchase_Id { get; set; }
        private string item_name = "Hello";
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
            set => UpdateObservable(ref _isPurchased, value);
        }
        public int Counter { get; set; }
        public Purchase_ItemsDTO(int counter)
        {
            Counter = counter;
        }
        public Purchase_ItemsDTO()
        {
            Counter = 0;
        }

    }
}
