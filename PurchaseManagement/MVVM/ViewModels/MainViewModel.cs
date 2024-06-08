using AutoMapper;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Windows.Input;
using PurchaseManagement.Commons;
using Patterns;
using PurchaseManagement.DataAccessLayer.Repository;

namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class LaodableMainViewModel<TItem>: Loadable<TItem> where TItem : PurchaseDto
    {
        public override int Index(TItem item)
        {
            return Items.ToList().FindIndex(i => i.Id == item.Id);
        }
        protected override void Reorder()
        {
            var data = GetItems().OrderByDescending(a => a.Purchase_Date).ToList();
            SetItems(data);
        }

    }
    public class MainViewModel: LaodableMainViewModel<PurchaseDto>
    {
        private readonly IGenericRepository<Purchase> _purchaseDB;
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value);
        }
        private Mapper mapper = MapperConfig.InitializeAutomapper();
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        private readonly IRepository _db;
        INotication notication;
        public ICommand AddCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public MainViewModel(IGenericRepository<Purchase> purchaseDB)
        {
            _purchaseDB = purchaseDB;
            IsSavebtnEnabled = true;
            _ = LoadItems();
            SetupCommands();
            
        }
        private void SetupCommands()
        {
            AddCommand = new Command(On_Add);
            DoubleClickCommand = new Command(On_DoubleClick);
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
            //ProductDto purchase_proxy_item;
            //Purchase purchase = await _db.GetFullPurchaseByDate(SelectedDate);
            //if(purchase != null)
            //{
            //    PurchaseStatistics stat = await _db.GetPurchaseStatistics(purchase.Id);
            //    purchase_proxy_item = stat == null ? new ProductDto(0) : new ProductDto(stat.PurchaseCount);
            //}
            //else
            //{
            //    purchase_proxy_item = new ProductDto(0);
            //}

            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", true },
                            { "Purchase_ItemsDTO", new ProductDto(0) }
                };
            await Shell.Current.GoToAsync(nameof(MarketFormPage), navigationParameter);
        }
        
        
        public override async Task LoadItems()
        {
            ShowActivity();
            IEnumerable<Purchase> _purchases = await _purchaseDB.GetAll();
            var data = _purchases.Select(mapper.Map<PurchaseDto>).ToList();
            SetItems(data); 
            HideActivity();
        }
        //private List<PurchaseDto> TestLoad()
        //{
        //    List<PurchaseDto> p = [new("Hello", SelectedDate), new("Hello", SelectedDate), new("Hello", SelectedDate), new("Hello", SelectedDate)];
        //    return p;
        //}
        
    }
}
