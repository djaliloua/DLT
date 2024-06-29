using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using MauiNavigationHelper.NavigationLib.Models;
using MauiNavigationHelper.NavigationLib.Abstractions;
using PurchaseManagement.Utilities;
using Patterns;
using AutoMapper;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using PurchaseManagement.Commons.Notifications;

namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: ProductDto
    {
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(item => item.Id).ToList();
            SetItems(data);
        }

    }
    public class ProductItemsViewModel: PurchaseItemsViewModelLoadable<ProductDto>, INavigatedEvents
    {
        #region Private properties
        private readonly INavigationService navigationService;
        private readonly IPurchaseRepository _purchaseDB;
        private readonly IGenericRepository<ProductStatistics> _statisticsDB;
        private readonly IGenericRepository<MarketModels.Location> _locationRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly INotification _messageBox;
        private readonly Mapper mapper = MapperConfig.InitializeAutomapper();
        private ExportContext<ProductDto> exportContext;
        #endregion

        #region Public Properties
        private PurchasesDTO _puchases;
        public PurchasesDTO Purchases
        {
            get => _puchases;
            set
            {
                _puchases = value;
                if (value != null)
                {
                    _ = LoadItems();
                }
            }
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
        public ProductItemsViewModel(IProductRepository productRepository,
            IGenericRepository<ProductStatistics> statisticsDB,
            IPurchaseRepository purchaseDB,
            INavigationService navigationService,
            ExportContext<ProductDto> context,
            IGenericRepository<MarketModels.Location> locationRepository)
        {
            _productRepository = productRepository;
            _statisticsDB = statisticsDB;
            _purchaseDB = purchaseDB;
            exportContext = context;
            this.navigationService = navigationService;
            _locationRepository = locationRepository;
            _notification = new ToastNotification();
            _messageBox = new MessageBoxNotification();
            RegisterToMessage();
            CommandSetup();
        }
        #endregion

        #region Handlers
        private async void OnBackCommand(object parameter)
        {
            await navigationService.Navigate("..");
        }
        public async void OnOpenAnalyticCommand(object parameter)
        {
            var navigationParameters = new NavigationParameters
            {
                { "product", GetItems() }
            };
            await navigationService.Navigate(nameof(ProductAnalytics), navigationParameters);
            
        }
        private void OnExportToPdfCommand(object parameter)
        {
            exportContext.ExportTo("", GetItems());
        }
        private async void On_GetMap(object parameter)
        {
            ShowActivity();
            ProductDto productDto = parameter as ProductDto;
            SelectedItem = productDto;
            if (IsSelected)
            {
                Microsoft.Maui.Devices.Sensors.Location location = await ProductViewModelUtility.GetCurrentLocation();
                if (await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchase)
                {
                    // Update Product with its corresponding location
                    var loc = mapper.Map<MarketModels.Location>(location);
                    SelectedItem.Location = mapper.Map<LocationDto>(loc);
                    loc.Purchase_Id = SelectedItem.PurchaseId;
                    loc.Purchase_Item_Id = SelectedItem.Id;
                    SelectedItem.IsLocation = SelectedItem.Location != null;
                    loc = await _locationRepository.SaveOrUpdateItem(loc);
                    SelectedItem.Location_Id = loc.Id;
                    await _productRepository.SaveOrUpdateItem(mapper.Map<Product>(SelectedItem));
                    await _notification.ShowNotification("Got location");
                    // Update UI
                    UpdateUI();
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
                var mapper = MapperConfig.InitializeAutomapper();
                ProductDto proxy = mapper.Map<ProductDto>(productDto);
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "IsSave", false },
                            {"Purchase_ItemsDTO", proxy }
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
                if (SelectedItem.Location != null)
                    await ProductViewModelUtility.NavigateToBuilding25(mapper.Map<Microsoft.Maui.Devices.Sensors.Location>(SelectedItem.Location));
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
                    await _productRepository.DeleteItem(mapper.Map<Product>(SelectedItem));
                    // Update Stat
                    ProductStatistics purchaseStatistics = await _statisticsDB.GetItemById(SelectedItem.PurchaseId);
                    await _statisticsDB.SaveOrUpdateItem(await StatisticRepoUtility.CreateOrUpdatePurchaseStatistics(purchaseStatistics));
                    //
                    if(SelectedItem.Location_Id != 0)
                    {
                        MarketModels.Location loc = await _locationRepository.GetItemById(SelectedItem.Location_Id);
                        if (loc != null)
                            await _locationRepository.DeleteItem(loc);
                    }

                    // Update UI
                    var p = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(p));
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
                //SelectedItem = null;
                await navigationService.Navigate(nameof(PurchaseItemDetails), navigationParameters);
            }
        }
        #endregion

        #region Private Methods
        private void RegisterToMessage()
        {
            WeakReferenceMessenger.Default.Register<ProductDto, string>(this, "update", async (sender, p) =>
            {
                if (p.Purchase is PurchasesDTO purchase)
                {
                    p.PurchaseId = purchase.Id;
                    var x = mapper.Map<Product>(p);
                    await _productRepository.SaveOrUpdateItem(x);
                    var purchaseX = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(purchaseX));
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
            GetMapCommand = new Command(On_GetMap);
            OpenAnalyticCommand = new Command(OnOpenAnalyticCommand);
            ExportToPdfCommand = new Command(OnExportToPdfCommand);
        }
        
        private async void UpdateUI()
        {
            var purchase = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(purchase));
        }
        
        #endregion

        public void ResetSelectedItem()
        {
            SelectedItem = null;
        }
        public override async Task LoadItems()
        {
            ShowActivity();
            await Task.Delay(1);
            SetItems(Purchases.Products);
            HideActivity();
        }

        public Task OnNavigatedTo(NavigationParameters parameters)
        {
            Purchases = parameters.GetValue<PurchasesDTO>("purchase");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFrom(NavigationParameters parameters)
        {
           return Task.CompletedTask;
        }
    }
}
