

using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class PurchasesDTO:BaseViewModel
    {
        private int _purchase_Id;
        public int Purchase_Id
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
        public string Purchase_Date
        {
            get => _purchase_Date;
            set => UpdateObservable(ref _purchase_Date, value);
        }
        private IList<Purchase_ItemsDTO> _purchase_Items;
        public IList<Purchase_ItemsDTO> Purchase_Items
        {
            get => _purchase_Items;
            set => UpdateObservable(ref _purchase_Items, value);
        }
        private int purchase_Stat_Id;
        public int Purchase_Stats_Id
        {
            get => purchase_Stat_Id;
            set => UpdateObservable(ref purchase_Stat_Id, value);
        }
        private PurchaseStatisticsDTO _purchaseSatistics;
        public PurchaseStatisticsDTO PurchaseStatistics
        {
            get => _purchaseSatistics;
            set => UpdateObservable(ref _purchaseSatistics, value);
        }
    }
}
