using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemsViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository _db;
        public ObservableCollection<Purchase_Items> Purchase_Items { get; }
        private Purchase_Items _selected_Purchase_Item;
        public Purchase_Items Selected_Purchase_Item
        {
            get => _selected_Purchase_Item;
            set => UpdateObservable(ref _selected_Purchase_Item, value);
        }
        public PurchaseItemsViewModel(IRepository _db)
        {
            this._db = _db;
            Purchase_Items = new ObservableCollection<Purchase_Items>();
        }
        public async Task LoadPurchaseItemsAsync(int purchaseId)
        {
            Purchase_Items.Clear();
            var purchase_items = await _db.GetAllPurchaseItemById(purchaseId);
            foreach (var item in purchase_items)
            {
                Purchase_Items.Add(item);
            }
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                Purchases purchase = query["purchase"] as Purchases;
                await LoadPurchaseItemsAsync(purchase.Purchase_Id);
            }
        }
    }
}
