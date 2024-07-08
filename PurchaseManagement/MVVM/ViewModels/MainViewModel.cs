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
        public override bool ItemExist(TItem item)
        {
            return Items.FirstOrDefault(x => x.Id==item.Id) != null;
        }
        protected override void Reorder()
        {
            var data = GetItems().OrderByDescending(a => a.PurchaseDate).ToList();
            SetItems(data);
        }
        public override void SetItems(IList<TItem> items)
        {
            var data = items.OrderByDescending(a => a.PurchaseDate).ToList();
            base.SetItems(data);
        }

    }
    public class MainViewModel: LaodableMainViewModel<PurchaseDto>
    {
        #region Private Properties
        private readonly INavigationService _navigationService;
        private readonly IPurchaseRepository _purchaseRepository;
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
        public MainViewModel(IPurchaseRepository db, INavigationService navigationService)
        {
            _purchaseRepository = db;   
            _navigationService = navigationService;
            IsSavebtnEnabled = true;
            Init();
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
        private async void Init()
        {
            await LoadItems();
        }
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
                    await _navigationService.Navigate(nameof(ProductsPage), navigationParameters);
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
            if(await _purchaseRepository.GetPurchaseByDate(SelectedDate) is Purchase purchase)
            {
                ProductStatistics stat = purchase.ProductStatistics;
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
            IEnumerable<Purchase> _purchases = await _purchaseRepository.GetAllItemsAsync();
            var data = _purchases.Adapt<List<PurchaseDto>>();
            SetItems(data);
            HideActivity();
        }
        #endregion

    }
}
