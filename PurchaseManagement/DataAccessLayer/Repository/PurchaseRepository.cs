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
            Purchase purchase = await _context.Set<Purchase>().FirstOrDefaultAsync(p => p.PurchaseDate.Equals($"{date:yyyy-MM-dd}"));
            return purchase;
        }

        public Task<List<PurchaseDto>> GetAllAsDtos()
        {
            return Task.FromResult(_context.Set<Purchase>().ProjectToType<PurchaseDto>().ToList());
        }
        public PurchaseDto RemoveProduct(Product product)
        {
            Purchase p = _context.Purchases.FirstOrDefault(x => x.Id==product.Purchase.Id);
            p.Products.Remove(p.Products.FirstOrDefault(x => x.Id==product.Id));
            Save();
            return p.Adapt<PurchaseDto>();
        }
    }
}
