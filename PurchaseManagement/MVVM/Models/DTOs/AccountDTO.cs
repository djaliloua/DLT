using MVVM;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class AccountDTO:BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id; 
            set => UpdateObservable(ref _id, value);
        }
        private double _money;
        public double Money
        {
            get => _money;
            set => UpdateObservable(ref _money, value);
        }
        private DateTime _dataTime;
        public DateTime DateTime
        {
            get => _dataTime;
            set => UpdateObservable(ref _dataTime, value);
        }
        private string _day;
        public string Day
        {
            get => _day;
            set => UpdateObservable(ref _day, value);
        }
        public AccountDTO(double _money)
        {
            Money = _money;
            DateTime = DateTime.Now;
            Day = DateTime.Now.ToString("dddd");
        }
        public AccountDTO(DateTime _date, double _money)
        {
            Money = _money;
            DateTime = _date;
            Day = _date.ToString("dddd");
        }
        public AccountDTO()
        {

        }
    }
}
