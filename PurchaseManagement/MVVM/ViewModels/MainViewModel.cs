using AutoMapper;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public ObservableCollection<PurchasesDTO> Purchases { get; }
        
        private PurchasesDTO _selectedPurchase;
        public PurchasesDTO SelectedPurchase
        {
            get => _selectedPurchase;
            set => UpdateObservable(ref _selectedPurchase, value);
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value);
        }
        private Mapper mapper;
        bool CanOpen => SelectedPurchase != null;
        private readonly IRepository _db;
        public ICommand AddCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public MainViewModel(IRepository db)
        {
            Show = true;
            _db = db;
            mapper = MapperConfig.InitializeAutomapper();
            Purchases = new ObservableCollection<PurchasesDTO>();
            _ = Load();
            AddCommand = new Command(On_Add);
            DoubleClickCommand = new Command(On_DoubleClick);
            Show = false;
        }
        private async void On_DoubleClick(object sender)
        {
            if(CanOpen)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "purchase", SelectedPurchase }
                        };
                SelectedDate = DateTime.Parse(SelectedPurchase.Purchase_Date);
                await Shell.Current.GoToAsync(nameof(PurchaseItemsPage), navigationParameter);
            }
        }
        private async void On_Add(object sender)
        {
            Purchase_ItemsDTO purchase_proxy_item;
            Purchases purchase = await _db.GetPurchasesByDate(SelectedDate);
            if(purchase != null)
            {
                PurchaseStatistics stat = await _db.GetPurchaseStatistics(purchase.Purchase_Id);
                purchase_proxy_item = stat == null ? new Purchase_ItemsDTO(0) : new Purchase_ItemsDTO(stat.PurchaseCount);
            }
            else
            {
                purchase_proxy_item = new Purchase_ItemsDTO(0);
            }

            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", true },
                            { "Purchase_ItemsDTO", purchase_proxy_item }
                };
            await Shell.Current.GoToAsync(nameof(MarketFormPage), navigationParameter);
        }
        public async Task Load()
        {
            if (_db != null)
            {
                await LoadPurchasesAsync();
            }
        }
        public async Task LoadPurchasesAsync()
        {
            ShowProgressBar();
            Purchases.Clear();
            IEnumerable<Purchases> _purchases = await Task.Run(_db.GetAllPurchases);
            foreach (Purchases purchase in _purchases)
            {
                purchase.PurchaseStatistics = await _db.GetPurchaseStatistics(purchase.Purchase_Id);
                purchase.Purchase_Items = await Task.Run(async () => await _db.GetAllPurchaseItemById(purchase.Purchase_Id));
                for (int i = 0; i < purchase.Purchase_Items.Count; i++)
                {
                    purchase.Purchase_Items[i].Location = await _db.GetMarketLocationAsync(purchase.Purchase_Id, purchase.Purchase_Items[i].Item_Id);
                }
                Purchases.Add(mapper.Map<PurchasesDTO>(purchase));
            }
            HideProgressBar();
        }
    }
}
