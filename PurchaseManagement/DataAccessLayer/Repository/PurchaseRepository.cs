using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Abstractions;
using Microsoft.EntityFrameworkCore;
using Mapster;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ExtensionMethods;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class PurchaseRepository : GenericRepository<Purchase>,IPurchaseRepository
    {
        public async Task<Purchase> GetPurchaseByDate(DateTime date)
        {
            Purchase purchase = await _context.Purchases.FirstOrDefaultAsync(p => p.PurchaseDate.Equals($"{date:yyyy-MM-dd}"));
            return purchase;
        }

        public Task<IList<PurchaseDto>> GetAllAsDtos()
        {
            return Task.FromResult(_context.Purchases.ToList().ToDto());
        }
        public PurchaseDto RemoveProduct(Product product)
        {
            Purchase p = _context.Purchases.FirstOrDefault(x => x.Id==product.Purchase.Id);
            p.Products.Remove(p.Products.FirstOrDefault(x => x.Id==product.Id));
            Save();
            return p.ToDto();
        }
    }
}
