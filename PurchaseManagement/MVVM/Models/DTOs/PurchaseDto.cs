

using MVVM;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class PurchaseDto:BaseViewModel
    {
        private int _purchase_Id;
        public int Id
        {
            get => _purchase_Id;
            set => UpdateObservable(ref _purchase_Id, value);
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => UpdateObservable(ref _title, value);
        }
        private string _purchase_Date;
        public string PurchaseDate
        {
            get => _purchase_Date;
            set => UpdateObservable(ref _purchase_Date, value);
        }
        private ObservableCollection<ProductDto> _purchase_Items;
        public ObservableCollection<ProductDto> Products
        {
            get => _purchase_Items;
            set => UpdateObservable(ref _purchase_Items, value);
        }

        private ProductStatisticsDto _purchaseSatistics;
        public ProductStatisticsDto ProductStatistics
        {
            get => _purchaseSatistics;
            set => UpdateObservable(ref _purchaseSatistics, value);
        }
    }
}
