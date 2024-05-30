using AutoMapper;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Windows.Input;
using Patterns;

namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class LaodableMainViewModel<TItem>: Loadable<TItem> where TItem : PurchasesDTO
    {
        public TItem GetItemByDate(DateTime date)
        {
            TItem item = GetItems().FirstOrDefault(p => p.Purchase_Date.Equals($"{date:yyyy-MM-dd}"));
            return item;
        }
        public DateTime DateTime { get; set; }
        public override void UpdateItem(TItem item)
        {
            TItem item1 = GetItemByDate(DateTime);
            if (item1 != null)
            {
                item1.Purchase_Items = item.Purchase_Items;
                item1.PurchaseStatistics = item.PurchaseStatistics;
                item1.Purchase_Stats_Id = item.Purchase_Stats_Id;
            }
            
        }
        public override void Reorder()
        {
            var data = GetItems().OrderByDescending(a => a.Purchase_Date).ToList();
            SetItems(data);
        }

    }
    public class MainViewModel: LaodableMainViewModel<PurchasesDTO>
    {
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value, () =>
            {
                DateTime = value;
            });
        }
        private Mapper mapper;
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        private readonly IRepository _db;
        public ICommand AddCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public MainViewModel(IRepository db)
        {
            _db = db;
            mapper = MapperConfig.InitializeAutomapper();
            AddCommand = new Command(On_Add);
            DoubleClickCommand = new Command(On_DoubleClick);
#if WINDOWS
            _ = Load();
#endif
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !Show;
        }
        private async void On_DoubleClick(object sender)
        {
            if(IsSelected)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "purchase", SelectedItem }
                        };
                SelectedDate = DateTime.Parse(SelectedItem.Purchase_Date);
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
                await LoadItems();
            }
        }
        
        public override async Task LoadItems()
        {
            ShowProgressBar();
            IList<Purchases> _purchases = await _db.GetAllPurchases();
            var data = _purchases.Select(mapper.Map<PurchasesDTO>).ToList();
            SetItems(data); 
            HideProgressBar();
        }
        
    }
}
