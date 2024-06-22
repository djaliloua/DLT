using PurchaseManagement.Pages;

namespace PurchaseManagement
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(ProductAnalytics), typeof(ProductAnalytics));
            Routing.RegisterRoute(nameof(PurchaseItemDetails), typeof(PurchaseItemDetails));
            Routing.RegisterRoute(nameof(MarketFormPage), typeof(MarketFormPage));
        }
    }
}