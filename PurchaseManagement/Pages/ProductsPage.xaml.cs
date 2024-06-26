using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.Pages;

public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        ViewModelLocator.ProductItemsViewModel.ResetSelectedItem();
        return base.OnBackButtonPressed();
    }
}