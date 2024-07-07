using Microsoft.EntityFrameworkCore;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.DataAccessLayer.Contexts
{
    public class RepositoryContext:DbContext
    {
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLocation> ProductLocations { get; set; }
        public DbSet<ProductStatistics> ProductStatistics { get; set; }
        public DbSet<Account> Accounts { get; set; }
        private string DatabasePurchase;
        public RepositoryContext()
        {
            DatabasePurchase = Path.Combine(FileSystem.AppDataDirectory, "Purchase.db3");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite($"Data Source={DatabasePurchase}");
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
