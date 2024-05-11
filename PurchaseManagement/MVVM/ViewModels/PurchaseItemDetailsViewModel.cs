using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemDetailsViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository _db;
        private Purchase_ItemsDTO _purchaseDetails;
        public Purchase_ItemsDTO PurchaseDetails
        {
            get => _purchaseDetails;
            set => UpdateObservable(ref _purchaseDetails, value);
        }
        public PurchaseItemDetailsViewModel(IRepository _db)
        {
            this._db = _db;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                PurchaseDetails = query["details"] as Purchase_ItemsDTO;
            }
        }
    }
}
