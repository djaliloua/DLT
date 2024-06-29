using PurchaseManagement.MVVM.Models.MarketModels;


namespace PurchaseManagement.DataAccessLayer.Abstractions
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        Task<Purchase> GetPurchaseByDate(DateTime date);
        Task<Purchase> GetFullPurchaseByDate(DateTime date);
    }
}
