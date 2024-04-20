using PurchaseManagement.MVVM.Models;
using PurchaseManagement.Services;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemsPage : ContentPage
{
	public PurchaseItemsPage()
	{
		InitializeComponent();
        NavigatedTo += PurchaseItemsPage_NavigatedTo;
	}
    private async void PurchaseItemsPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (RegisterViewModels.PurchaseItemsViewModel.Purchases is Purchases p)
        {
            await RegisterViewModels.PurchaseItemsViewModel.LoadPurchaseItemsAsync(p.Purchase_Id);
        }
    }
}