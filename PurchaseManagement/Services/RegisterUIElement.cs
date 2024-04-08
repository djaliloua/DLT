using PurchaseManagement.Pages;

namespace PurchaseManagement.Services
{
    public static class RegisterUIElement
    {
        public static MauiAppBuilder RegisterUIPages(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            //mauiAppBuilder.Services.AddSingleton<>();
            return mauiAppBuilder;
        }
    }
}
