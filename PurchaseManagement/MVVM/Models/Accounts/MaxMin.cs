namespace PurchaseManagement.MVVM.Models.Accounts
{
    public class MaxMin
    {
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public MaxMin(DateTime _dt, double val)
        {
            Value = val;
            DateTime = _dt;
        }
        public MaxMin()
        {
            Value = 0;
        }
    }
}
