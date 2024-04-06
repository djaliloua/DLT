using PurchaseManagement.DataAccessLayer;

namespace PurchaseManagement.Services
{
    public static class RegisterDbContext
    {
        public static MauiAppBuilder RegisterDbContextService(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IRepository, Repository>();
            return mauiAppBuilder;
        }
    }
}
