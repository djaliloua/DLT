namespace PurchaseManagement.Commons
{
    public class MessageBoxNotification : INotification
    {
        public async Task ShowNotification(string message)
        {
            await Shell.Current.DisplayAlert("Message", message, "Cancel");
        }
    }
}
