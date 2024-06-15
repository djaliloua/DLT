namespace PurchaseManagement.Pages
{
    public partial class AccountPage : ContentPage
    {
        partial void ChangedHandler(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            var x = listView.Handler.PlatformView;
        }
    }
}
