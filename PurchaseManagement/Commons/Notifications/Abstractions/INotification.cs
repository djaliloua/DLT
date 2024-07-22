namespace PurchaseManagement.Commons.Notifications.Abstractions
{
    public interface INotification
    {
        Task ShowNotification(string message);
    }
}
