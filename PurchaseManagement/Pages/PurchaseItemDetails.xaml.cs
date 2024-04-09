using PurchaseManagement.Services;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemDetails : ContentPage
{
	public PurchaseItemDetails()
	{
		InitializeComponent();
		BindingContext = RegisterViewModels.GetPurchaseItemDetailsViewModel();
    }
}