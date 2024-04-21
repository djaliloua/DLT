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
            if(RegisterViewModels.PurchaseItemDetailsViewModel.PurchaseDetails is Purchase_Items purch)
            {
                Dispatcher.Dispatch(() =>
                {
                    listview.ScrollTo(RegisterViewModels.PurchaseItemsViewModel.Purchase_Items.FirstOrDefault(item => item.Item_Id == purch.Item_Id), 
                        ScrollToPosition.End, false);
                });
                
            }
            
        }
    }
}