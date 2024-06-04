using CommunityToolkit.Maui.Core;
using MVVM;
using CommunityToolkit.Maui.Alerts;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using AutoMapper;
using Microsoft.Maui.ApplicationModel;

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
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        public int Counter = 0;
        Mapper mapper = MapperConfig.InitializeAutomapper();

        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public MarketFormViewModel(IRepository _db)
        {
            db = _db;
            CommandSetup();
            
        }
        #endregion
        private void CommandSetup()
        {
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
        protected override void OnShow()
        {
            IsSavebtnEnabled = !Show;
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

            if (ViewModelLocator.PurchaseItemsViewModel.IsSelected)
            {
                await _update(mapper.Map<Purchases>(ViewModelLocator.PurchaseItemsViewModel.Purchases));
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            ShowProgressBar();
            Purchases purchase = new Purchases("test", ViewModelLocator.MainViewModel.SelectedDate);
            if (await db.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                await _savePurchaseItemAndStatDb(purchases);
            }
            else
            {
                await _saveDb(purchase);
            }
            Counter++;
            await MakeToast(Counter);
            HideProgressBar();
        }
        private async Task _update(Purchases purchase)
        {
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(PurchaseItem);
            PurchaseStatistics stat = await db.GetPurchaseStatistics(purchase.Purchase_Id);
            purchase.PurchaseStatistics = stat;
            m_purchase_item.Purchase = purchase;
            m_purchase_item.Purchase_Id = purchase.Purchase_Id;
            await db.SavePurchaseItemAsync(m_purchase_item);
            var s = await db.SavePurchaseStatisticsItemAsyn(purchase, stat);
            purchase.Purchase_Stats_Id = s.Id;
            
            _ = await db.SavePurchaseAsync(purchase);

            // Update UI
            var p = await db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            Update(mapper.Map<PurchasesDTO>(p));
        }
       
        private async Task _savePurchaseItemAndStatDb(Purchases purchase)
        {
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(PurchaseItem);
            PurchaseStatistics stat = await db.GetPurchaseStatistics(purchase.Purchase_Id);
            purchase.PurchaseStatistics = stat;
            m_purchase_item.Purchase = purchase;
            m_purchase_item.Purchase_Id = purchase.Purchase_Id;
            await db.SavePurchaseItemAsync(m_purchase_item);
            var s = await db.SavePurchaseStatisticsItemAsyn(purchase, stat);
            purchase.Purchase_Stats_Id = s.Id;
            await db.SavePurchaseAsync(purchase);

            // UI
            purchase = await db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            var p_DTO = mapper.Map<PurchasesDTO>(purchase);
            Update(p_DTO);

        }
        
        private void Update(PurchasesDTO newObj)
        {
            ViewModelLocator.MainViewModel.UpdateItem(newObj);
        }
        private async Task _saveDb(Purchases purchase)
        {
            purchase = await db.SavePurchaseAsync(purchase);
            PurchaseStatistics m_purchaseStatistics = new(purchase.Purchase_Id, 1, PurchaseItem.Item_Price, PurchaseItem.Item_Quantity);
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(PurchaseItem);
            //
            m_purchase_item.Purchase_Id = purchase.Purchase_Id;
            purchase.Purchase_Items.Add(m_purchase_item);
            purchase.PurchaseStatistics = m_purchaseStatistics;
            m_purchase_item.Purchase = purchase;
            
            await db.SavePurchaseItemAsync(m_purchase_item);
            //
            var p = await db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            ViewModelLocator.MainViewModel.AddItem(mapper.Map<PurchasesDTO>(p));
            
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
