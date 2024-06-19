using PurchaseManagement.Commons;

namespace PurchaseManagement.Pages;

public partial class ProductAnalytics : ContentPage
{
	public ProductAnalytics()
	{
		InitializeComponent();
	}
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }
    //protected override bool OnBackButtonPressed()
    //{
    //    NavigationParameters.Clear();
    //    _ = Shell.Current.GoToAsync("..");
    //    //ShellNavigationQueryParameters
    //    return true;
    //}
}