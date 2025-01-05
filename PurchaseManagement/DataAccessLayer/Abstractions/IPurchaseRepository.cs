using PurchaseManagement.MVVM.Models.ViewModel;
using Repository.Interface;
using Models.Market;


namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        Task<Purchase> GetPurchaseByDate(DateTime date);
        Task<IList<PurchaseViewModel>> GetAllAsDtos();
    }
}
