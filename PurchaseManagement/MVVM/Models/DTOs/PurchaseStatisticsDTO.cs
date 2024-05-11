using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class PurchaseStatisticsDTO:BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id; 
            set => UpdateObservable(ref _id, value);
        }
        private int _purchase_Id;
        public int Purchase_Id
        {
            get => _purchase_Id;
            set => UpdateObservable(ref _purchase_Id, value);
        }
        private int _purchaseCount;
        public int PurchaseCount
        {
            get => _purchaseCount;
            set => UpdateObservable(ref _purchaseCount, value);
        }
        private long _totalPrice;
        public long TotalPrice
        {
            get => _totalPrice;
            set => UpdateObservable(ref _totalPrice, value);
        }
        private long _totalQuantity;
        public long TotalQuantity
        {
            get => _totalQuantity;
            set => UpdateObservable(ref _totalQuantity, value);
        }
        public PurchaseStatisticsDTO(int p_id, int p_count, long total_price, long total_quantity)
        {
            Purchase_Id = p_id;
            PurchaseCount = p_count;
            TotalPrice = total_price;
            TotalQuantity = total_quantity;
        }
        public PurchaseStatisticsDTO()
        {

        }
    }
}
