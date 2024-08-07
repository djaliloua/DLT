using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Abstractions;
using Microsoft.EntityFrameworkCore;
using Mapster;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class PurchaseRepository : GenericRepository<Purchase>,IPurchaseRepository
    {
        public async Task<Purchase> GetPurchaseByDate(DateTime date)
        {
            Purchase purchase = await _table.FirstOrDefaultAsync(p => p.PurchaseDate.Equals($"{date:yyyy-MM-dd}"));
            return purchase;
        }

        public Task<List<PurchaseDto>> GetAllAsDtos()
        {
            return Task.FromResult(_table.ProjectToType<PurchaseDto>().ToList());
        }
    }
}
