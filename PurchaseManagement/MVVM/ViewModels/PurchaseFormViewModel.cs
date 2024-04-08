using Mopups.Services;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.Services;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class Purchase_ItemsProxy:BaseViewModel
    {
        private string item_name = "hello";
        public string Item_Name
        {
            get => item_name;
            set => UpdateObservable(ref item_name, value);
        }
        private int item_price;
        public int Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }private int item_quantity;
        public int Item_Quantity
        {
            get => item_quantity;   
            set => UpdateObservable(ref item_quantity, value);  
        }
    }
    public class PurchaseFormViewModel:BaseViewModel
    {
        private readonly IRepository db;
        private Purchase_ItemsProxy _purchaseItem;
        public Purchase_ItemsProxy PurchaseItem
        {
            get => _purchaseItem;
            set => UpdateObservable(ref _purchaseItem, value);
        }
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public PurchaseFormViewModel(IRepository _db)
        {
            PurchaseItem = new();
            CancelCommand = new Command(On_Cancel);
            SaveCommand = new Command(On_Save);
            db = _db;
        }
        private async void On_Save(object sender)
        {
            IEnumerable<Purchases> purchases = await db.GetPurchasesByDate();
          
            if (purchases.Count() == 1)
            {
                await db.SavePurchaseItemAsync(new(purchases.ElementAt(0).Purchase_Id,
                    PurchaseItem.Item_Name,
                    PurchaseItem.Item_Price,
                    PurchaseItem.Item_Quantity));
                return;
            }

            Purchases purchase = new Purchases("test");
            await db.SavePurchaseAsync(purchase);
            await db.SavePurchaseItemAsync(new(purchase.Purchase_Id,
                    PurchaseItem.Item_Name,
                    PurchaseItem.Item_Price,
                    PurchaseItem.Item_Quantity));
            MainViewModel vm = RegisterViewModels.GetMainViewModel();
            await vm.LoadPurchasesAsync();

        }
        private async void On_Cancel(object sender)
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
