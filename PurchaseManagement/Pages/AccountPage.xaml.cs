namespace PurchaseManagement.Pages;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
        
    }
    partial void ChangedHandler(object sender, EventArgs e);
    private void listview_HandlerChanged(object sender, EventArgs e) => ChangedHandler(sender, e);


}