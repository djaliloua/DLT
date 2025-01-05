using PurchaseManagement.MVVM.Models.ViewModel;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.Commons;
using MauiNavigationHelper.NavigationLib.Models;
using MauiNavigationHelper.NavigationLib.Abstractions;
using PurchaseManagement.Utilities;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Commons.Notifications.Implementations;
using Mapster;
using Patterns.Implementations;
using Patterns.Abstractions;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.ExtensionMethods;
using MVVM;
using Models.Market;
using Repository;

namespace PurchaseManagement.MVVM.ViewModels.PurchasePage
{
    public class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: ProductViewModel
    {
        public PurchaseItemsViewModelLoadable(ILoadService<TItem> loadService):base(loadService)
        {

        }
        public override void SetItems(IEnumerable<TItem> items)
        {
            var data = items.OrderByDescending(item => item.Id).ToList();
            base.SetItems(data);
        }

    }
    public class LoadProductService : ILoadService<ProductViewModel>
    {
        public IList<ProductViewModel> Reorder(IList<ProductViewModel> items)
        {
            return items;
        }
    }
    public class ProductListViewModel: PurchaseItemsViewModelLoadable<ProductViewModel>
    {
        #region Private properties
        private readonly INavigationService _navigationService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly INotification _notification;
        private readonly INotification _messageBox;
        #endregion

        #region Public Properties
        private PurchaseViewModel _puchases;
        public PurchaseViewModel Purchases
        {
            get => _puchases;
            set => UpdateObservable(ref _puchases, value, async () =>
            {
                ShowActivity();
                await Task.Run(async () => await LoadItems(Purchases.Products));
                HideActivity();
            });
        }
        private bool _isLocAvailable;
        public bool IsLocAvailable
        {
            get => _isLocAvailable;
            set => UpdateObservable(ref _isLocAvailable, value);
        }
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        #endregion

        #region Commands
        public ICommand OpenAnalyticCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion
        public ProductListViewModel(INavigationService navigationService, 
            ILoadService<ProductViewModel> loadService):base(loadService)
        {
            _navigationService = navigationService;
            _notification = new ToastNotification();
            _messageBox = new MessageBoxNotification();
            RegisterToMessage();
            CommandSetup();
        }

        #region Private Methods
        private void RegisterToMessage()
        {
            WeakReferenceMessenger.Default.Register<ProductViewModel, string>(this, "update", async (sender, p) =>
            {
                if (p.Purchase is PurchaseViewModel purchaseX)
                {
                    var purchase = ViewModelLocator.PurchasesListViewModel.GetItemByDate();
                    if (purchase is not null)
                        await MarketFormViewModelUtility.UpdateProductItem(purchase, p);
                }
            });
        }
        private void CommandSetup()
        {
            DoubleClickCommand = new Command(OnDoubleClick);
            OpenCommand = new Command(OnOpen);
            DeleteCommand = new Command(OnDelete);
            OpenMapCommand = new Command(OnOpenMap);
            EditCommand = new Command(OnEdit);
            GetMapCommand = new Command(OnGetMap);
        }

        #endregion

        #region Handlers
        private async void OnGetMap(object parameter)
        {
            ShowActivity();
            ProductViewModel productDto = parameter as ProductViewModel;
            if (IsSelected)
            {
                SelectedItem = productDto;
                Location location = await ProductViewModelUtility.GetCurrentLocation();
                if (ViewModelLocator.PurchasesListViewModel.GetItemByDate() is PurchaseViewModel purch)
                {
                    var loc = location.Adapt<ProductLocation>();
                    SelectedItem.ProductLocation = loc.ToVM<ProductLocation, LocationViewModel>();
                    SelectedItem.IsLocation = SelectedItem.ProductLocation != null;
                    await MarketFormViewModelUtility.UpdateProductItem(purch, SelectedItem);
                    await _notification.ShowNotification("Got location");
                }

            }
            else
                await _messageBox.ShowNotification("Please select the item first");
            HideActivity();
        }
        private async void OnEdit(object parameter)
        {
            ProductViewModel productDto = parameter as ProductViewModel;
            if (IsSelected)
            {
                SelectedItem = productDto;
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", false },
                            {"Purchase_ItemsDTO", SelectedItem },
                            {"currentDate", Purchases.PurchaseDate }
                        };

                await Shell.Current.GoToAsync(nameof(MarketFormPage), navigationParameter);
            }
            else
                await _messageBox.ShowNotification("Please select the item first");
        }
        private async void OnOpenMap(object parameter)
        {
            ProductViewModel productDto = parameter as ProductViewModel;
            if (IsSelected)
            {
                SelectedItem = productDto;
                if (SelectedItem.ProductLocation != null)
                    await ProductViewModelUtility.NavigateToBuilding25(SelectedItem.ProductLocation.Adapt<Location>());
                else
                    await Shell.Current.DisplayAlert("Message", "Get location", "Cancel");
            }
            else
                await _messageBox.ShowNotification("Please select the item first");
        }
        private async void OnDelete(object parameter)
        {
            ProductViewModel productDto = parameter as ProductViewModel;
            if (IsSelected)
            {
                SelectedItem = productDto;
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    using var repo = new PurchaseRepository();
                    var p = repo.RemoveProduct(SelectedItem.ToVM<ProductViewModel, Product>());
                    p.UpdateStatistics();
                    await ViewModelUtility.SaveAndUpdateUI(p.ToVM<PurchaseViewModel, Purchase>());
                    await _notification.ShowNotification($"{SelectedItem.Item_Name} deleted");
                    DeleteItem(SelectedItem);
                }
            }
            else
                await _messageBox.ShowNotification("Please select the item first");
        }
        private async void OnOpen(object parameter)
        {
            await Task.Delay(1);
        }
        private async void OnDoubleClick(object parameter)
        {
            if (IsSelected)
            {
                var navigationParameters = new NavigationParameters
            {
                { "details", SelectedItem }
            };
                await _navigationService.Navigate(nameof(PurchaseItemDetails), navigationParameters);
            }
        }
        #endregion
    }
    public class ProductItemsViewModel:BaseViewModel, IQueryAttributable
    {
        #region Private properties
        private readonly INavigationService _navigationService;
        private ExportContext<ProductViewModel> _exportContext;

        #endregion

        #region Public Properties
        private ProductListViewModel _productListViewModel;
        public ProductListViewModel ProductListViewModel
        {
            get => _productListViewModel;
            set => UpdateObservable(ref _productListViewModel, value);
        }
        private bool _isLocAvailable;
        public bool IsLocAvailable
        {
            get => _isLocAvailable;
            set => UpdateObservable(ref _isLocAvailable, value);
        }
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        #endregion

        #region Commands
        public ICommand OpenAnalyticCommand { get; private set; }
        public ICommand ExportToPdfCommand { get; private set; }
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public ProductItemsViewModel(IPurchaseRepository purchaseDB, 
            INavigationService navigationService,
            ExportContext<ProductViewModel> context)
        {
            _navigationService = navigationService;
            _exportContext = context;
            ProductListViewModel = ViewModelLocator.ProductListViewModel;
            CommandSetup();
        }
        #endregion

        #region Handlers
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                ProductListViewModel.Purchases = query.GetValue<PurchaseViewModel>("purchase");
            }
        }
        private async void OnBackCommand(object parameter)
        {
            await _navigationService.Navigate("..");
        }
        public async void OnOpenAnalyticCommand(object parameter)
        {
            var navigationParameters = new NavigationParameters
            {
                { "product", ProductListViewModel.GetItems() }
            };
            await _navigationService.Navigate(nameof(ProductAnalytics), navigationParameters);
            
        }
        private void OnExportToPdfCommand(object parameter)
        {
            _exportContext.ExportTo("", ProductListViewModel.GetItems());
        }
        
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            OpenAnalyticCommand = new Command(OnOpenAnalyticCommand);
            ExportToPdfCommand = new Command(OnExportToPdfCommand);
        }
        
        #endregion
        
    }
}
