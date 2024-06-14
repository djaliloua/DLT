namespace PurchaseManagement.Commons
{
    public interface INotification
    {
        Task ShowNotification(string message);
    }
}
