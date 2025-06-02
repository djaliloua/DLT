using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Models.Account;
using Models.Market;

namespace DataBaseContexts
{
    public class RepositoryContext : DbContext
    {
        //cd PurchaseManagement
        //dotnet ef database update --context RepositoryContext --project ../DataBaseContexts
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLocation> ProductLocations { get; set; }
        public DbSet<ProductStatistics> ProductStatistics { get; set; }
        public DbSet<Account> Accounts { get; set; }
        private string DatabasePurchase;
        public RepositoryContext()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (OperatingSystem.IsWindows())
            {
                IConfiguration Configuration = new ConfigurationBuilder()
                .AddUserSecrets<RepositoryContext>()
                .Build();
                DatabasePurchase = Configuration["local_db_folder"];
            }
            else
            {
                DatabasePurchase = Path.Combine(appDataPath, "Management.db3");
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePurchase}");
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.ConfigureWarnings(warn => warn.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));
        }

    }
}
