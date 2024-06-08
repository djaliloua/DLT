using Microsoft.EntityFrameworkCore;
using PurchaseManagement.Market.API.Models;

namespace PurchaseManagement.Market.API
{
    public class RepositoryContext:DbContext
    {
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ProductStatistics> ProductStatistics { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> option):base(option)
        {
            
        }
    }
}
