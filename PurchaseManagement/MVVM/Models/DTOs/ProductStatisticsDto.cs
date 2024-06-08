using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class ProductStatisticsDto:BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id; 
            set => UpdateObservable(ref _id, value);
        }
        private int _purchaseCount;
        public int ProductCount
        {
            get => _purchaseCount;
            set => UpdateObservable(ref _purchaseCount, value);
        }
        private double _totalPrice;
        public double TotalProductsPrice
        {
            get => _totalPrice;
            set => UpdateObservable(ref _totalPrice, value);
        }
        private double _totalQuantity;
        public double TotalProductsQuantity
        {
            get => _totalQuantity;
            set => UpdateObservable(ref _totalQuantity, value);
        }
        private int _purchase_Id;
        public int PurchaseId
        {
            get => _purchase_Id;
            set => UpdateObservable(ref _purchase_Id, value);
        }
        private PurchaseDto _purchase;
        public PurchaseDto Purchase
        {
            get => _purchase;
            set => UpdateObservable(ref _purchase, value);
        }
        public ProductStatisticsDto(int p_id, int p_count, double total_price, double total_quantity)
        {
            PurchaseId = p_id;
            ProductCount = p_count;
            TotalProductsPrice = total_price;
            TotalProductsQuantity = total_quantity;
        }
        public ProductStatisticsDto()
        {

        }
    }
}
