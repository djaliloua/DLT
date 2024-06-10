using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Pages;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
        Loaded += AccountPage_Loaded;
        
	}

    private void AccountPage_Loaded(object sender, EventArgs e)
    {

    }
}