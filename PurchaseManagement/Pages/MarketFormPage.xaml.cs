using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Pages;

public partial class MarketFormPage : ContentPage
{
	public MarketFormPage()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        ViewModelLocator.ProductItemsViewModel.ResetSelectedItem();
        return base.OnBackButtonPressed();
    }
}