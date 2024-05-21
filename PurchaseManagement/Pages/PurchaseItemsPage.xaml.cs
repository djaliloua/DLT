using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Diagnostics;

namespace PurchaseManagement.Pages;

public partial class PurchaseItemsPage : ContentPage
{
	public PurchaseItemsPage()
	{
		InitializeComponent();
        //Loaded += PurchaseItemsPage_Loaded;
	}

    private async void PurchaseItemsPage_Loaded(object sender, EventArgs e)
    {
        if (ViewModelLocator.PurchaseItemsViewModel.Purchases is PurchasesDTO)
        {
            await ViewModelLocator.PurchaseItemsViewModel.LoadItems();
        }
    }

    
}