using PurchaseManagement.Services;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemForm 
{
	public PurchaseItemForm()
	{
		InitializeComponent();
		BindingContext = RegisterViewModels.GetPurchaseFormViewModel();
    }
}