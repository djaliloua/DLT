using PurchaseManagement.DataAccessLayer.Abstractions;
using Microsoft.EntityFrameworkCore;
using PurchaseManagement.MVVM.Models.ViewModel;
using Repository.Implementation;
using Models.Market;
using Repository;

namespace PurchaseManagement.DataAccessLayer.Repository
{
    public class PurchaseRepository : GenericRepositoryViewModel<Purchase, PurchaseViewModel>,IPurchaseRepository
    {
        public PurchaseRepository()
        {
           
        }
        public async Task<Purchase> GetPurchaseByDate(DateTime date)
        {
            Purchase purchase = await _table.FirstOrDefaultAsync(p => p.PurchaseDate.Equals($"{date:yyyy-MM-dd}"));
            return purchase;
        }

        public async Task<IList<PurchaseViewModel>> GetAllAsDtos()
        {
            return await GetAllAsDtos();
        }
        public PurchaseViewModel RemoveProduct(Product product)
        {
            Purchase p = _table.FirstOrDefault(x => x.Id==product.Purchase.Id);
            p.Products.Remove(p.Products.FirstOrDefault(x => x.Id==product.Id));
            //Save();
            return p.ToVM<Purchase, PurchaseViewModel>();
        }
    }
}
