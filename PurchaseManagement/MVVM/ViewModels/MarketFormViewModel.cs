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
using System.Diagnostics;

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

            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
            {
                await _update(purchases);
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            ShowProgressBar();
            Purchases purchase = new Purchases("test", ViewModelLocator.MainViewModel.SelectedDate);
            if (await db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
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
            for(int i = 0; i < purchase.Purchase_Items.Count; i++)
            {
                if (purchase.Purchase_Items[i].Item_Id == m_purchase_item.Item_Id)
                {
                    purchase.Purchase_Items[i].Item_Description = m_purchase_item.Item_Description;
                    purchase.Purchase_Items[i].Item_Quantity = m_purchase_item.Item_Quantity;
                    purchase.Purchase_Items[i].Purchase = m_purchase_item.Purchase;
                    purchase.Purchase_Items[i].IsPurchased = m_purchase_item.IsPurchased;
                    purchase.Purchase_Items[i].Item_Price = m_purchase_item.Item_Price;
                    purchase.Purchase_Items[i].Location = m_purchase_item.Location;
                    purchase.Purchase_Items[i].Location_Id = m_purchase_item.Location_Id;
                    purchase.Purchase_Items[i].Item_Id = m_purchase_item.Item_Id;
                    purchase.Purchase_Items[i].Item_Name = m_purchase_item.Item_Name;
                    break;
                }
            }
            purchase = await db.SavePurchaseAsync(purchase);

            // Update UI
            UpdateById(mapper.Map<PurchasesDTO>(purchase));
        }
       
        private async Task _savePurchaseItemAndStatDb(Purchases purchase)
        {
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(PurchaseItem);
            purchase.Purchase_Items.Add(m_purchase_item);
            PurchaseStatistics stat = await db.GetPurchaseStatistics(purchase.Purchase_Id);
            purchase.PurchaseStatistics = stat;
            purchase = await db.SavePurchaseAsync(purchase);


            // Update UI

            UpdateById(mapper.Map<PurchasesDTO>(purchase));

        }
        
        private void UpdateById(PurchasesDTO newObj)
        {
            foreach (PurchasesDTO p in ViewModelLocator.MainViewModel.Purchases)
            {
                if(p.Purchase_Id == newObj.Purchase_Id)
                {
                    p.Purchase_Items = newObj.Purchase_Items;
                    p.PurchaseStatistics = newObj.PurchaseStatistics;
                    break;
                }
            }
        }
        private async Task _saveDb(Purchases purchase)
        {
            purchase = await db.SavePurchaseAsync(purchase);
            PurchaseStatistics m_purchaseStatistics = new(purchase.Purchase_Id, 1, PurchaseItem.Item_Price, PurchaseItem.Item_Quantity);
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(PurchaseItem);
            //
            m_purchase_item.Purchase = purchase;
            m_purchase_item.Purchase_Id = purchase.Purchase_Id;
            purchase.Purchase_Items.Add(m_purchase_item);
            purchase.PurchaseStatistics = m_purchaseStatistics;
            purchase = await db.SavePurchaseAsync(purchase);
            //
            ViewModelLocator.MainViewModel.Purchases.Add(mapper.Map<PurchasesDTO>(purchase));
            var data = ViewModelLocator.MainViewModel.Purchases.OrderByDescending(p => p.Purchase_Date).ToArray();
            ViewModelLocator.MainViewModel.Purchases.Clear();
            for (int i=0; i<data.Count(); i++)
            {
                ViewModelLocator.MainViewModel.Purchases.Add(data[i]);
            }
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
