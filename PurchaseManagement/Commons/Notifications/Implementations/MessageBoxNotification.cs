using PurchaseManagement.Commons.Notifications.Abstractions;

namespace PurchaseManagement.Commons.Notifications.Implementations
{
    public class MessageBoxNotification : INotification
    {
        public async Task ShowNotification(string message)
        {
            await Shell.Current.DisplayAlert("Message", message, "Cancel");
        }
    }
}
