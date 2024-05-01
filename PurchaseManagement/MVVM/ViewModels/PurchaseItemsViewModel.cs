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
        public Purchases Purchases;
        private Purchase_Items _selected_Purchase_Item;
        public Purchase_Items Selected_Purchase_Item
        {
            get => _selected_Purchase_Item;
            set => UpdateObservable(ref _selected_Purchase_Item, value);
        }
        bool CanOpen => Selected_Purchase_Item != null;
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public PurchaseItemsViewModel(IRepository _db)
        {
            this._db = _db;
            Purchase_Items = new ObservableCollection<Purchase_Items>();
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
        }
         private async void On_Open(object parameter)
        {
            await NavigateToBuilding25();
        }
        public async Task NavigateToBuilding25()
        {
            var location = new Location(47.645160, -122.1306032);
            var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // No map application available to open
            }
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
        public async Task LoadPurchaseItemsAsync(int purchaseId)
        {
            Purchase_Items.Clear();
            var purchase_items = await Task.Run(async() => await _db.GetAllPurchaseItemById(purchaseId));
            for(int i = 0; i <  purchase_items.Count; i++)
            {
                Purchase_Items.Add(purchase_items[i]);
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                Purchases = query["purchase"] as Purchases;
                
            }
        }
    }
}
