namespace PurchaseManagement.Services
{
    public static class RegisterUIElement
    {
        public static MauiAppBuilder RegisterUIPages(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            return mauiAppBuilder;
        }
    }
}
