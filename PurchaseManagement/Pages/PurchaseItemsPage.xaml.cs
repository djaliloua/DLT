using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Diagnostics;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemsPage : ContentPage
{
	public PurchaseItemsPage()
	{
		InitializeComponent();
        NavigatedTo += PurchaseItemsPage_NavigatedTo;
	}
    
    // TODO: Show data on UI by batch(20) and trigger additional data loading when scroll reaches last item
    private async void PurchaseItemsPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        
        if (ViewModelLocator.PurchaseItemsViewModel.Purchases is PurchasesDTO p)
        {
            await ViewModelLocator.PurchaseItemsViewModel.LoadPurchaseItemsDTOAsync(p.Purchase_Items);
        }
    }
}