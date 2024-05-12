using CommunityToolkit.Maui.Core;
using MVVM;
using CommunityToolkit.Maui.Alerts;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using AutoMapper;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository db;
        private Purchase_ItemsDTO _purchaseItem;
        public Purchase_ItemsDTO PurchaseItem
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
            PurchasesDTO p_dto = ViewModelLocator.MainViewModel.GetPurchasesDTOByDate();
            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                await db.SavePurchaseItemAsync(mapper.Map<Purchase_Items>(PurchaseItem));
                purchaseStatistics = await db.GetPurchaseStatistics(purchases.Purchase_Id);
                purchaseStatistics.PurchaseCount = await db.CountPurchaseItems(purchases.Purchase_Id);
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "price");
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "quantity");
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);
                ViewModelLocator.PurchaseItemsViewModel.Purchases.PurchaseStatistics = mapper.Map<PurchaseStatisticsDTO>(purchaseStatistics);
                // Update UI
                p_dto.PurchaseStatistics = mapper.Map<PurchaseStatisticsDTO>(purchaseStatistics);
                p_dto.Purchase_Items.Clear();
                IList<Purchase_Items> d = await Task.Run(async () => await db.GetAllPurchaseItemById(purchases.Purchase_Id));
                for (int i = 0; i < d.Count(); i++)
                {
                    p_dto.Purchase_Items.Add(mapper.Map<Purchase_ItemsDTO>(d[i]));
                }
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            List<PurchasesDTO> Temp = new();
            List<Purchase_ItemsDTO> Temp_Purchase_Items = new();
            Purchases purchase = new Purchases("test", ViewModelLocator.MainViewModel.SelectedDate);
            var item = mapper.Map<Purchase_Items>(PurchaseItem);
            PurchasesDTO p_dto = ViewModelLocator.MainViewModel.GetPurchasesDTOByDate();
            
            PurchaseStatistics purchaseStatistics;
            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                item.Purchase_Id = purchases.Purchase_Id;
                item.Purchase = purchases;
                await db.SavePurchaseItemAsync(item);
                purchaseStatistics = await db.GetPurchaseStatistics(purchases.Purchase_Id);
                purchaseStatistics.PurchaseCount = await db.CountPurchaseItems(purchases.Purchase_Id);
                purchaseStatistics.TotalPrice = await db.GetTotalValue(purchases, "Price");
                purchaseStatistics.TotalQuantity = await db.GetTotalValue(purchases, "Quantity");
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);

                // Update UI data
                p_dto.PurchaseStatistics = mapper.Map<PurchaseStatisticsDTO>(purchaseStatistics);
                p_dto.Purchase_Items.Clear();
                IList<Purchase_Items> d = await db.GetAllPurchaseItemById(purchases.Purchase_Id);
                for (int i = 0; i < d.Count(); i++)
                {
                    p_dto.Purchase_Items.Add(mapper.Map<Purchase_ItemsDTO>(d[i]));
                }

            }
            else
            {
                await db.SavePurchaseAsync(purchase);
                item = mapper.Map<Purchase_Items>(PurchaseItem);
                
                await db.SavePurchaseItemAsync(item);
                purchaseStatistics = new(purchase.Purchase_Id, 1,
                       PurchaseItem.Item_Price,
                        PurchaseItem.Item_Quantity);
                
                await db.SavePurchaseStatisticsItemAsyn(purchaseStatistics);

                // Update UI data
                purchase.PurchaseStatistics = purchaseStatistics;
                item.Purchase_Id = purchase.Purchase_Id;
                item.Purchase = purchase;
                p_dto = mapper.Map<PurchasesDTO>(purchase);
                p_dto.Purchase_Items = new List<Purchase_ItemsDTO>() { mapper.Map<Purchase_ItemsDTO>(item) };
                ViewModelLocator.MainViewModel.Purchases.Add(p_dto);
                Temp.AddRange(ViewModelLocator.MainViewModel.Purchases);
                ViewModelLocator.MainViewModel.Purchases.Clear();
                var d = Temp.OrderByDescending(x => x.Purchase_Date).ToArray();
                for(int i=0; i<d.Length; i++)
                {
                    ViewModelLocator.MainViewModel.Purchases.Add(d[i]);
                }
               
            }
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
                PurchaseItem = query["Purchase_ItemsDTO"] as Purchase_ItemsDTO;
                Counter = PurchaseItem.Counter;
            }
        }
    }
}
