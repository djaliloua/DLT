using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.MarketModels;
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

namespace PurchaseManagement.MVVM.ViewModels.PurchasePage
{
    public class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: ProductDto
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
    public class LoadProductService : ILoadService<ProductDto>
    {
        public IList<ProductDto> Reorder(IList<ProductDto> items)
        {
            return items;
        }
    }
    public class ProductItemsViewModel: PurchaseItemsViewModelLoadable<ProductDto>, INavigatedEvents
    {
        #region Private properties
        private readonly INavigationService _navigationService;
        private readonly INotification _notification;
        private readonly INotification _messageBox;
        private readonly IPurchaseRepositoryApi _genericRepositoryApi;
        private ExportContext<ProductDto> _exportContext;
        #endregion

        #region Public Properties
        private PurchaseDto _puchases;
        public PurchaseDto Purchases
        {
            get => _puchases;
            set => UpdateObservable(ref  _puchases, value, async () =>
            {
                ShowActivity();
                await Task.Run(async() => await LoadItems(Purchases.Products));
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
        public ProductItemsViewModel(
            INavigationService navigationService,
            IPurchaseRepositoryApi genericRepositoryApi,
            ExportContext<ProductDto> context,
            ILoadService<ProductDto> loadService
            ):base(loadService)
        {
            _exportContext = context;
            _navigationService = navigationService;
            _genericRepositoryApi = genericRepositoryApi;
            _notification = new ToastNotification();
            _messageBox = new MessageBoxNotification();
            RegisterToMessage();
            CommandSetup();
        }
        #endregion

        #region Handlers
        private async void OnBackCommand(object parameter)
        {
            await _navigationService.Navigate("..");
        }
        public async void OnOpenAnalyticCommand(object parameter)
        {
            var navigationParameters = new NavigationParameters
            {
                { "product", GetItems() }
            };
            await _navigationService.Navigate(nameof(ProductAnalytics), navigationParameters);
            
        }
        private void OnExportToPdfCommand(object parameter)
        {
            _exportContext.ExportTo("", GetItems());
        }
        private async void OnGetMap(object parameter)
        {
            ShowActivity();
            ProductDto productDto = parameter as ProductDto;
            SelectedItem = productDto;
            if (IsSelected)
            {
                Location location = await ProductViewModelUtility.GetCurrentLocation();
                if (ViewModelLocator.MainViewModel.GetItemByDate() is PurchaseDto purch)
                {
                    var loc = location.Adapt<ProductLocation>();
                    SelectedItem.ProductLocation = loc.Adapt<LocationDto>();
                    SelectedItem.IsLocation = SelectedItem.ProductLocation != null;
                    await ViewModelUtility.SaveAndUpdateUI(purch);
                    await _notification.ShowNotification("Got location");
                }

            }
            else
                await _messageBox.ShowNotification("Please select the item first");
            HideActivity();
        }
        private async void OnEdit(object parameter)
        {
            ProductDto productDto = parameter as ProductDto;
            SelectedItem = productDto;
            if (IsSelected)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", false },
                            {"Purchase_ItemsDTO", SelectedItem }
                        };
                
                await Shell.Current.GoToAsync(nameof(MarketFormPage), navigationParameter);
            }
            else
                await _messageBox.ShowNotification("Please select the item first");
        }
        private async void OnOpenMap(object parameter)
        {
            ProductDto productDto = parameter as ProductDto;
            SelectedItem = productDto;
            if (IsSelected)
            {
                
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
            ProductDto productDto = parameter as ProductDto;
            SelectedItem = productDto;
            if (IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    PurchaseDto purchase = ViewModelLocator.MainViewModel.GetItemByDate();
                    purchase.Remove(SelectedItem);
                    await ViewModelUtility.SaveAndUpdateUI(purchase);
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

        #region Private Methods
        private void RegisterToMessage()
        {
            WeakReferenceMessenger.Default.Register<ProductDto, string>(this, "update", async (sender, p) =>
            {
                if (p.Purchase is PurchaseDto purchaseX)
                {
                    var purchase = ViewModelLocator.MainViewModel.GetItemByDate();
                    await _genericRepositoryApi.SaveOrUpdate(purchase.Adapt<Purchase>());
                }
            });
        }
        private void CommandSetup()
        {
            BackCommand = new Command(OnBackCommand);
            DoubleClickCommand = new Command(OnDoubleClick);
            OpenCommand = new Command(OnOpen);
            DeleteCommand = new Command(OnDelete);
            OpenMapCommand = new Command(OnOpenMap);
            EditCommand = new Command(OnEdit);
            GetMapCommand = new Command(OnGetMap);
            OpenAnalyticCommand = new Command(OnOpenAnalyticCommand);
            ExportToPdfCommand = new Command(OnExportToPdfCommand);
        }
        #endregion

        public Task OnNavigatedTo(NavigationParameters parameters)
        {
            Purchases = parameters.GetValue<PurchaseDto>("purchase");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFrom(NavigationParameters parameters)
        {
           return Task.CompletedTask;
        }
    }
}
