namespace PurchaseManagement.Commons.Notifications
{
    public class MessageBoxNotification : INotification
    {
        public async Task ShowNotification(string message)
        {
            await Shell.Current.DisplayAlert("Message", message, "Cancel");
        }
    }
}
