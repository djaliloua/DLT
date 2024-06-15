using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Pages;

public partial class AccountAnalyticPage : ContentPage
{
	public AccountAnalyticPage()
	{
		InitializeComponent();
        NavigatedTo += AccountAnalyticPage_NavigatedTo;
	}

    private async void AccountAnalyticPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        await ViewModelLocator.AccountAnalyticViewModel.Load();
    }
}