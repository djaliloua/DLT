using CommunityToolkit.Maui.Core;
using MVVM;
using CommunityToolkit.Maui.Alerts;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using AutoMapper;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class Purchase_ItemsProxyViewModel:BaseViewModel
    {
        public int Item_Id { get; set; }
        public int Purchase_Id { get; set; }
        private string item_name = "Hello";
        public string Item_Name
        {
            get => item_name;
            set => UpdateObservable(ref item_name, value);
        }
        private long item_price;
        public long Item_Price
        {
            get => item_price;
            set => UpdateObservable(ref item_price, value);
        }
        private long item_quantity;
        public long Item_Quantity
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
        public int Counter { get; set; }
        public Purchase_ItemsProxyViewModel(int counter)
        {
            Counter = counter;
        }
        public Purchase_ItemsProxyViewModel()
        {
            
        }

    }
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository db;
        private Purchase_ItemsProxyViewModel _purchaseItem;
        public Purchase_ItemsProxyViewModel PurchaseItem
        {
            get => _purchaseItem;
            set => UpdateObservable(ref _purchaseItem, value);
        }
        private bool _isSave;
        public bool IsSave
        {
            get => _isSave;
            set => UpdateObservable(ref _isSave, value);    
        }
        public int Counter = 0;
        Mapper mapper;
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public MarketFormViewModel(IRepository _db)
        {
            db = _db;
            mapper = MapperConfig.InitializeAutomapper();
            PurchaseItem = new();
            CancelCommand = new Command(On_Cancel);
            SaveCommand = new Command(On_Save);
            BackCommand = new Command(On_Back);
            UpdateCommand = new Command(On_Update);
        }
        
        private async void On_Back(object parameter)
        {
            Counter = 0;
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
        private async void On_Update(object parameter)
        {
            PurchaseStatistics purchaseStatistics;
            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                await db.SavePurchaseItemAsync(mapper.Map<Purchase_Items>(PurchaseItem));
                purchaseStatistics = await db.GetPurchaseStatistics(purchases.Purchase_Id);
                purchaseStatistics.PurchaseCount = await db.CountPurchaseItems(purchases.Purchase_Id);
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "price");
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "quantity");
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);

            }
            await ViewModelLocator.MainViewModel.LoadPurchasesAsync();
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            Purchases purchase = new Purchases("test", ViewModelLocator.MainViewModel.SelectedDate);
            var item = mapper.Map<Purchase_Items>(PurchaseItem);
            PurchaseStatistics purchaseStatistics;
            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                item.Purchase_Id = purchases.Purchase_Id;
                item.Purchase = purchases;
                await db.SavePurchaseItemAsync(item);
                purchaseStatistics = await db.GetPurchaseStatistics(purchases.Purchase_Id);
                purchaseStatistics.PurchaseCount = await db.CountPurchaseItems(purchases.Purchase_Id);
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "price");
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "quantity");
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);
               
            }
            else
            {
                await db.SavePurchaseAsync(purchase);
                item = mapper.Map<Purchase_Items>(PurchaseItem);
                item.Purchase_Id = purchase.Purchase_Id;
                item.Purchase = purchase;
                await db.SavePurchaseItemAsync(item);
                purchaseStatistics = new(purchase.Purchase_Id, 1,
                       PurchaseItem.Item_Price,
                        PurchaseItem.Item_Quantity);
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);
            }
            await ViewModelLocator.MainViewModel.LoadPurchasesAsync();
            Counter++;
            await MakeToast(Counter);
        }
        private async void On_Cancel(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count() > 0)
            {
                IsSave = (bool)query["IsSave"];
                PurchaseItem = query["Purchase_ItemsProxy"] as Purchase_ItemsProxyViewModel;
                Counter = PurchaseItem.Counter;
            }
        }
    }
}
