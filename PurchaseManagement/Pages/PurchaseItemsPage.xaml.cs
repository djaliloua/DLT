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
        //listview.Scrolled += Listview_Scrolled;
	}
    private void Listview_Scrolled(object sender, ScrolledEventArgs e)
    {
        listview.Measure(double.PositiveInfinity, double.PositiveInfinity);
        double contentHeight = listview.DesiredSize.Height;
        double totalscrollableheight = listview.Height - listview.Margin.Bottom - listview.Margin.Top;
        Trace.WriteLine(contentHeight);
        Trace.WriteLine(totalscrollableheight);
        Trace.WriteLine(e.ScrollY);
        
    }
    // TODO: Show data on UI by batch(20) and trigger additional data loading when scroll reaches last item
    private async void PurchaseItemsPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        
        if (ViewModelLocator.PurchaseItemsViewModel.Purchases is PurchasesDTO p)
        {
            await ViewModelLocator.PurchaseItemsViewModel.LoadPurchaseItemsAsync(p.Purchase_Id);
        }
    }
}