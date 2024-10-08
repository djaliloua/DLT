﻿using Microsoft.EntityFrameworkCore;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.DataAccessLayer.Contexts;
using PurchaseManagement.MVVM.Models.Accounts;


namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class AccountRepository : GenericRepository<Account>,IAccountRepository
    {
        public AccountRepository()
        {
            _context = new RepositoryContext();
            _context.Database.EnsureCreated();
        }
        public override async Task<IEnumerable<Account>> GetAllItemsAsync()
        {
            return await _context.Accounts.FromSql($"select *\r\nfrom Accounts acc\r\norder by acc.DateTime desc;").AsNoTracking().ToListAsync();
        }
        public async Task<IList<Statistics>> GetStatisticsAsync()
        {
            string sql = "select AC.DateTime, avg(AC.Money) AvgMoney, sum(AC.Money) TotalMoney, count(AC.Money) CountMoney\r\nfrom Accounts AC\r\ngroup by AC.Day\r\nOrder by AC.Day desc\r\n;";
            return await _context.Database.SqlQueryRaw<Statistics>(sql).AsNoTracking().ToListAsync();
        }
        public async Task<IList<MaxMin>> GetMinAsync()
        {
            string sql = "select AC.DateTime, min(AC.Money) Value\r\nfrom Accounts AC;";
            return await _context.Database.SqlQueryRaw<MaxMin>(sql).AsNoTracking().ToListAsync();
        }
        public async Task<IList<MaxMin>> GetMaxAsync()
        {
            string sql = "select AC.DateTime, max(AC.Money) Value\r\nfrom Accounts AC;";
            return await _context.Database.SqlQueryRaw<MaxMin>(sql).AsNoTracking().ToListAsync();
        }

        
    }
}
