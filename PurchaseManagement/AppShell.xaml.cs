using PurchaseManagement.Pages;

namespace PurchaseManagement
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PurchaseItemsPage), typeof(PurchaseItemsPage));
            Routing.RegisterRoute(nameof(PurchaseItemDetails), typeof(PurchaseItemDetails));
            Routing.RegisterRoute(nameof(MarketFormPage), typeof(MarketFormPage));
        }
    }
}