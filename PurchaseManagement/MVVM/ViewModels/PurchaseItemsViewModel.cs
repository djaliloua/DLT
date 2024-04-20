using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        bool CanOpen => Selected_Purchase_Item != null;
        public ICommand DoubleClickCommand { get; private set; }
        public PurchaseItemsViewModel(IRepository _db)
        {
            this._db = _db;
            Purchase_Items = new ObservableCollection<Purchase_Items>();
            DoubleClickCommand = new Command(On_DoubleClick);
        }
        private async void On_DoubleClick(object parameter)
        {
            if (CanOpen)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "details", Selected_Purchase_Item }
                        };
                await Shell.Current.GoToAsync(nameof(PurchaseItemDetails), navigationParameter);
            }
        }
        private async Task LoadPurchaseItemsAsync(int purchaseId)
        {
            Purchase_Items.Clear();
            var purchase_items = await Task.Run(async() => await _db.GetAllPurchaseItemById(purchaseId));
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
