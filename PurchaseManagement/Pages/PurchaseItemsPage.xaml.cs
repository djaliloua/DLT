using PurchaseManagement.Services;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemsPage : ContentPage
{
	public PurchaseItemsPage()
	{
		InitializeComponent();
		BindingContext = RegisterViewModels.GetPurchaseItemsViewModel();
	}
}