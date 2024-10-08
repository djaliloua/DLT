using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using System.Windows.Input;
using System.Diagnostics;
using PurchaseManagement.Commons;
using MauiNavigationHelper.NavigationLib.Abstractions;
using MauiNavigationHelper.NavigationLib.Models;
using Patterns.Implementations;
using Patterns.Abstractions;
using PurchaseManagement.DataAccessLayer.Repository;
using MVVM;
using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.MVVM.ViewModels.PurchasePage
{
    public class LaodableMainViewModel<TItem>: Loadable<TItem> where TItem : PurchaseDto
    {
        public LaodableMainViewModel(ILoadService<TItem> loadService):base(loadService)
        {
            
        }
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
        
        public override void SetItems(IEnumerable<TItem> items)
        {
            var data = items.OrderByDescending(a => a.PurchaseDate).ToList();
            base.SetItems(data);
        }

    }
    public class LoadPurchaseService : ILoadService<PurchaseDto>
    {
        public IList<PurchaseDto> Reorder(IList<PurchaseDto> items)
        {
            return items.OrderByDescending(a => a.PurchaseDate).ToList();
        }
    }
    public class PurchasesListViewModel: LaodableMainViewModel<PurchaseDto>
    {
        #region Private Properties

        #endregion
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value, () =>
            {
                DateTime = value;
            });
        }
        public ICommand RefreshCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public PurchasesListViewModel(ILoadService<PurchaseDto> loadService) : base(loadService)
        {
            Init();
            DoubleClickCommand = new Command(OnDoubleClick);
            RefreshCommand = new Command(OnRefresh);
        }
        private async void Init()
        {
            ShowActivity();
            List<Purchase> items = await _genericRepositoryApi.GetAllItems() ?? new List<Purchase>();
            await Task.Run(async () => await LoadItems(items.ToDto()));
            HideActivity();
        }
        private void OnRefresh(object parameter)
        {
            IsRefreshed = true;
            Init();
            IsRefreshed = false;
        }
        private async void OnDoubleClick(object sender)
        {
            SelectedItem = (PurchaseDto)sender;
            if (!Debugger.IsAttached)
            {
                // Do not delete this piece of code
                //FingerPrintAuthentification _authentification = new FingerPrintAuthentification();
                //await _authentification.Authenticate();
            }
            try
            {
                if (IsSelected)
                {
                    SelectedDate = DateTime.Parse(SelectedItem.PurchaseDate);
                    var navigationParameters = new NavigationParameters
                    {
                        { "purchase", SelectedItem }
                    };
                    await Shell.Current.GoToAsync(nameof(ProductsPage), navigationParameters);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
    public class MainViewModel: BaseViewModel
    {
        #region Private Properties
        private readonly INavigationService _navigationService;
        
        #endregion

        #region Public Properites
        public PurchasesListViewModel PurchasesListViewModel { get; private set; }
        
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        #endregion

        #region Commands
        
        public ICommand AddCommand { get; private set; }
        
        #endregion

        #region Constructor
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            IsSavebtnEnabled = true;
            PurchasesListViewModel = ViewModelLocator.PurchasesListViewModel; 
            CommandSetup();
        }
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            AddCommand = new Command(OnAdd);
        }
        
        #endregion

        #region Handlers
        
        
        private async void OnAdd(object sender)
        {
            ProductDto purchase_proxy_item;
            if(PurchasesListViewModel.GetItemByDate() is PurchaseDto purchase)
            {
                ProductStatisticsDto stat = purchase.ProductStatistics;
                purchase_proxy_item = Factory.CreateObject(stat);
            }
            else
            {
                purchase_proxy_item = Factory.CreateObject(0);
            }
            NavigationParametersTest.AddParameter("IsSave", true);
            NavigationParametersTest.AddParameter("Purchase_ItemsDTO", purchase_proxy_item);
            NavigationParametersTest.AddParameter("currentDate", DateTime.Now);
            await Shell.Current.GoToAsync(nameof(MarketFormPage), NavigationParametersTest.GetParameters());
        }
        #endregion
    }
}
