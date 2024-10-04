using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;


namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        Task<Purchase> GetPurchaseByDate(DateTime date);
        Task<IList<PurchaseDto>> GetAllAsDtos();
    }
}
