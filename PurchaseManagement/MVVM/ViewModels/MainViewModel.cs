using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Windows.Input;
using PurchaseManagement.DataAccessLayer.Abstractions;
using Patterns;
using PurchaseManagement.MVVM.Models.MarketModels;
using System.Diagnostics;
using PurchaseManagement.Commons;
using MauiNavigationHelper.NavigationLib.Abstractions;
using MauiNavigationHelper.NavigationLib.Models;
using Mapster;
namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class LaodableMainViewModel<TItem>: Loadable<TItem> where TItem : PurchaseDto
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
    public class MainViewModel: LaodableMainViewModel<PurchaseDto>
    {
        #region Private Properties
        private readonly INavigationService navigationService;
        private readonly IPurchaseRepository _purchaseDB;
        private readonly IGenericRepository<ProductStatistics> _statisticsDB;
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
        public MainViewModel(IPurchaseRepository db,
            INavigationService navigationService,
            IGenericRepository<ProductStatistics> statisticsDB)
        {
            _purchaseDB = db;   
            _statisticsDB = statisticsDB;
            this.navigationService = navigationService;
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
                    SelectedDate = DateTime.Parse(SelectedItem.PurchaseDate);
                    var navigationParameters = new NavigationParameters
                    {
                        { "purchase", SelectedItem }
                    };
                    await navigationService.Navigate(nameof(ProductsPage), navigationParameters);
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
            if(GetItemByDate() is PurchaseDto purchase)
            {
                ProductStatistics stat = await _statisticsDB.GetItemById(purchase.Id);
                purchase_proxy_item = Factory.CreateObject(stat.Adapt<ProductStatisticsDto>());
            }
            else
            {
                purchase_proxy_item = Factory.CreateObject(0);
            }
            NavigationParametersTest.AddParameter("IsSave", true);
            NavigationParametersTest.AddParameter("Purchase_ItemsDTO", purchase_proxy_item);
            await Shell.Current.GoToAsync(nameof(MarketFormPage), NavigationParametersTest.GetParameters());
        }
        #endregion

        #region public Overriden methods
        public override async Task LoadItems()
        {
            ShowActivity();
            IEnumerable<Purchase> _purchases = await _purchaseDB.GetAllItems();
            var data = _purchases.Adapt<List<PurchaseDto>>();
            SetItems(data);
            HideActivity();
        }
        #endregion

    }
}
