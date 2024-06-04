using Microsoft.EntityFrameworkCore;
using PurchaseManagement.API.Models;

namespace PurchaseManagement.API.DataAccessLayer
{
    public class AccountContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public AccountContext(DbContextOptions<AccountContext> options):base(options)
        {
        }
    }
}
