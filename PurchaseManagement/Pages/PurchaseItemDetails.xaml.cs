using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemDetails : ContentPage
{
	public PurchaseItemDetails()
	{
		InitializeComponent();
    }
    protected override bool OnBackButtonPressed()
    {
        ViewModelLocator.ProductItemsViewModel.ResetSelectedItem();
        return base.OnBackButtonPressed();
    }
}