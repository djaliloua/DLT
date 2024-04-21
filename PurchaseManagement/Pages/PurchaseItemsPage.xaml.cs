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
    // TODO: Show data on UI by batch(20) and trigger additional data loading when scroll reaches last item
    private async void PurchaseItemsPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (RegisterViewModels.PurchaseItemsViewModel.Purchases is Purchases p)
        {
            await RegisterViewModels.PurchaseItemsViewModel.LoadPurchaseItemsAsync(p.Purchase_Id);
            if (RegisterViewModels.PurchaseItemDetailsViewModel.PurchaseDetails is Purchase_Items purch)
            {
                await Task.Delay(10);
                Dispatcher.Dispatch(() =>
                {
                    listview.ScrollTo(RegisterViewModels.PurchaseItemsViewModel.Purchase_Items.FirstOrDefault(item => item.Item_Id == purch.Item_Id),
                        ScrollToPosition.End, false);
                });
            }
        }
    }
}