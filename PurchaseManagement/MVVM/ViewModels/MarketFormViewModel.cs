using CommunityToolkit.Maui.Core;
using MVVM;
using CommunityToolkit.Maui.Alerts;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.ServiceLocator;
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
        private string item_price;
        public string Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }
        private string item_quantity;
        public string Item_Quantity
        {
            get => item_quantity;   
            set => UpdateObservable(ref item_quantity, value);  
        }
        private string _item_desc;
        public string Item_Description
        {
            get => _item_desc;
            set => UpdateObservable(ref _item_desc, value);
        }
    }
    public class MarketFormViewModel:BaseViewModel
    {
        private readonly IRepository db;
        private Purchase_ItemsProxy _purchaseItem;
        public Purchase_ItemsProxy PurchaseItem
        {
            get => _purchaseItem;
            set => UpdateObservable(ref _purchaseItem, value);
        }
        private static int count = 0;
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public MarketFormViewModel(IRepository _db)
        {
            PurchaseItem = new();
            CancelCommand = new Command(On_Cancel);
            SaveCommand = new Command(On_Save);
            BackCommand = new Command(On_Back);
            db = _db;
        }
        private async void On_Back(object parameter)
        {
            count = 0;
            await Shell.Current.GoToAsync("..");
        }
        private async Task MakeToast(int count)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = $"{count} ";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
        private async void On_Save(object sender)
        {
            IEnumerable<Purchases> purchases = await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
            Purchases purchase = new Purchases("test", ViewModelLocator.MainViewModel.SelectedDate);
            PurchaseStatistics purchaseStatistics;
            if (purchases.Count() >= 1)
            {
                await db.SavePurchaseItemAsync(new(purchases.ElementAt(0).Purchase_Id,
                    PurchaseItem.Item_Name,
                    PurchaseItem.Item_Price,
                    PurchaseItem.Item_Quantity, PurchaseItem.Item_Description));
                purchaseStatistics = await db.GetPurchaseStatistics(purchases.ElementAt(0).Purchase_Id);
                purchaseStatistics.PurchaseCount = await db.CountPurchaseItems(purchases.ElementAt(0).Purchase_Id);
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases.ElementAt(0), "price");
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases.ElementAt(0), "quantity");
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);
               
            }
            else
            {
                await db.SavePurchaseAsync(purchase);
                await db.SavePurchaseItemAsync(new(purchase.Purchase_Id,
                        PurchaseItem.Item_Name,
                        PurchaseItem.Item_Price,
                        PurchaseItem.Item_Quantity, PurchaseItem.Item_Description));
                purchaseStatistics = new(purchase.Purchase_Id, "1",
                        PurchaseItem.Item_Price,
                        PurchaseItem.Item_Quantity);
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);
            }
            await ViewModelLocator.MainViewModel.LoadPurchasesAsync();
            count++;
            await MakeToast(count);
        }
        private async void On_Cancel(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
