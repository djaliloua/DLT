using AutoMapper;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Windows.Input;
using Patterns;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.DataAccessLayer.Repository;
using System.Diagnostics;
namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class LaodableMainViewModel<TItem>: Loadable<TItem> where TItem : PurchasesDTO
    {
        public TItem GetItemByDate()
        {
            TItem item = GetItems().FirstOrDefault(p => p.PurchaseDate.Equals($"{DateTime:yyyy-MM-dd}"));
            return item;
        }
        public DateTime DateTime { get; set; }
        protected override int Index(TItem item)
        {
            TItem item1 = GetItemByDate();
            return base.Index(item1);
        }
        
        protected override void Reorder()
        {
            var data = GetItems().OrderByDescending(a => a.PurchaseDate).ToList();
            SetItems(data);
        }

    }
    public class MainViewModel: LaodableMainViewModel<PurchasesDTO>
    {
        #region Private Properties
        private readonly IPurchaseRepository _purchaseDB;
        private readonly IGenericRepository<PurchaseStatistics> _statisticsDB;
        #endregion

        #region Public Properites
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
        #endregion

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        #endregion

        #region Constructor
        public MainViewModel(IPurchaseRepository db, IGenericRepository<PurchaseStatistics> statisticsDB)
        {
            _purchaseDB = db;   
            _statisticsDB = statisticsDB;
            mapper = MapperConfig.InitializeAutomapper();   
            IsSavebtnEnabled = true;
            _ = LoadItems();
            CommandSetup();
        }
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            AddCommand = new Command(On_Add);
            DoubleClickCommand = new Command(On_DoubleClick);
        }
        #endregion

        #region Handlers

        private async void On_DoubleClick(object sender)
        {
            try
            {
                if (IsSelected)
                {
                    Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "purchase", SelectedItem }
                        };
                    SelectedDate = DateTime.Parse(SelectedItem.PurchaseDate);
                    await Shell.Current.GoToAsync(nameof(ProductsPage), navigationParameter);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        
        private async void On_Add(object sender)
        {
            ProductDto purchase_proxy_item;
            if(GetItemByDate() is PurchasesDTO purchase)
            {
                PurchaseStatistics stat = await _statisticsDB.GetItemById(purchase.Purchase_Id);
                purchase_proxy_item = Factory.CreateObject(mapper.Map<ProductStatisticsDto>(stat));
            }
            else
            {
                purchase_proxy_item = Factory.CreateObject(0);
            }

            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", true },
                            { "Purchase_ItemsDTO", purchase_proxy_item }
                };
            
            await Shell.Current.GoToAsync(nameof(MarketFormPage), navigationParameter);
        }
        #endregion

        #region public Overriden methods
        public override async Task LoadItems()
        {
            ShowActivity();
            IEnumerable<Purchase> _purchases = await _purchaseDB.GetAllItems();
            var data = _purchases.Select(mapper.Map<PurchasesDTO>).ToList();
            SetItems(data);
            HideActivity();
        }
        #endregion

    }
}
