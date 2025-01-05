namespace Models.Account
{
    public class Account : BaseEntity
    {
        public double Money { get; set; }
        public DateTime DateTime { get; set; }
        public string Day { get; set; }
        public Account(double _money)
        {
            Money = _money;
            DateTime = DateTime.Now;
            Day = DateTime.Now.ToString("dddd");
        }
        public Account(DateTime _date, double _money)
        {
            Money = _money;
            DateTime = _date;
            Day = _date.ToString("dddd");
        }
        public Account()
        {

        }
    }
}
