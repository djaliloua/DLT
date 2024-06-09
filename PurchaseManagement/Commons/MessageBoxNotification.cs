namespace PurchaseManagement.Commons
{
    public class MessageBoxNotification : INotification
    {
        public async void ShowNotification(string message)
        {
            await Shell.Current.DisplayAlert(message, "Please select the item first", "Cancel");
        }
    }
}
