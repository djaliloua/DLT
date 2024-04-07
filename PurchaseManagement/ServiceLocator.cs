namespace PurchaseManagement
{
    public static class ServiceLocator
    {
        public static T GetService<T>() => Application.Current.Handler.MauiContext.Services.GetRequiredService<T>();
    }
}
