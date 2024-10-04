using ManagPassWord.MVVM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ManagPassWord.DataAcessLayer.Contexts
{
    public class RepositoryContext:DbContext
    {
        public DbSet<Web> Users { get; set; }
        public DbSet<DebtModel> Debts { get; set; }
        public DbSet<Password> Passwords { get; set; }

        private string DatabasePurchase;
        public RepositoryContext()
        {
            DatabasePurchase = Path.Combine(FileSystem.AppDataDirectory, "Purchase.db3");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePurchase}");
            optionsBuilder.UseLazyLoadingProxies(true);
            optionsBuilder.ConfigureWarnings(warn => warn.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));
        }
    }
}
