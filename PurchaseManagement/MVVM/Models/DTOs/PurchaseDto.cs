using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class PurchaseDto:BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => UpdateObservable(ref _id, value);
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => UpdateObservable(ref _title, value);
        }
        private string _purchase_Date;
        public string Purchase_Date
        {
            get => _purchase_Date;
            set => UpdateObservable(ref _purchase_Date, value);
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
        private ICollection<ProductDto> _productItems;
        public ICollection<ProductDto> Products
        {
            get => _productItems;
            set => UpdateObservable(ref _productItems, value);
        }
        public PurchaseDto(string title, DateTime dt)
        {
            Title = title;
            Purchase_Date = dt.ToString("yyyy-MM-dd");
            ProductCount = 40;
            TotalProductsPrice = 200000;
            TotalProductsQuantity = 10;
            //AddItem();
        }
        public PurchaseDto()
        {
            
        }
        private void AddItem()
        {
            Products ??= new List<ProductDto>();
            for (int i = 0; i < ProductCount; i++)
            {
                Products.Add(new ProductDto());
            }
        }
    }
}
