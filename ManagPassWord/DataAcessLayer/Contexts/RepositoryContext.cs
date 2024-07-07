using ManagPassWord.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagPassWord.DataAcessLayer.Contexts
{
    public class RepositoryContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DebtModel> Debts { get; set; }

        private string DatabasePurchase;
        public RepositoryContext()
        {
            DatabasePurchase = Path.Combine(FileSystem.AppDataDirectory, "Purchase.db3");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePurchase}");
            optionsBuilder.UseLazyLoadingProxies(true);
        }
    }
}
