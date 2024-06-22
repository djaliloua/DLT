using PurchaseManagement.NavigationLib.Abstractions;
using PurchaseManagement.NavigationLib.Services;

namespace PurchaseManagement.NavigationLib.Models
{
    public static class BackButtonNavigator
    {
        public static bool HandleBackButtonPressed()
        {
            var navigationService = ServiceResolver.Resolve<INavigationService>();

            _ = navigationService.GoBack();

            // On Android and Windows, prevent the default back button behaviour
            return true;
        }
    }
}
