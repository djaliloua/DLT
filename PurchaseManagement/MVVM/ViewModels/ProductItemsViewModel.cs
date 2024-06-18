using AutoMapper;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using System.Diagnostics;
using System.Windows.Input;
using Patterns;
using CommunityToolkit.Mvvm.Messaging;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;

namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: ProductDto
    {
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(item => item.Item_Id).ToList();
            SetItems(data);
        }

    }
    public class ProductItemsViewModel: PurchaseItemsViewModelLoadable<ProductDto>, IQueryAttributable
    {
        #region Private properties
        private readonly IPurchaseRepository _purchaseDB;
        private readonly IGenericRepository<PurchaseStatistics> _statisticsDB;
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
        #endregion

        #region Constructor
        public ProductItemsViewModel(IProductRepository productRepository,
            IGenericRepository<PurchaseStatistics> statisticsDB,
            IPurchaseRepository purchaseDB,
            ExportContext<ProductDto> context,
            IGenericRepository<MarketModels.Location> locationRepository)
        {
            _productRepository = productRepository;
            _statisticsDB = statisticsDB;
            _purchaseDB = purchaseDB;
            exportContext = context;
            _locationRepository = locationRepository;
            _notification = new ToastNotification();
            _messageBox = new MessageBoxNotification();
            RegisterToMessage();
            CommandSetup();
        }
        #endregion

        #region Handlers
        public async void OnOpenAnalyticCommand(object parameter)
        {
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "product", GetItems() },
                            
                        };
            await Shell.Current.GoToAsync(nameof(ProductAnalytics), navigationParameter);
        }
        private void OnExportToPdfCommand(object parameter)
        {
            exportContext.ExportTo("", GetItems());
        }
        private async void On_GetMap(object parameter)
        {
            ShowActivity();
            if (IsSelected)
            {
                Microsoft.Maui.Devices.Sensors.Location location = await GetCurrentLocation();
                if (await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchase)
                {
                    // Update Product with its corresponding location
                    var loc = mapper.Map<MarketModels.Location>(location);
                    SelectedItem.Location = mapper.Map<LocationDto>(loc);
                    loc.Purchase_Id = SelectedItem.PurchaseId;
                    loc.Purchase_Item_Id = SelectedItem.Item_Id;
                    SelectedItem.IsLocation = SelectedItem.Location != null;
                    loc = await _locationRepository.SaveOrUpdateItem(loc);
                    SelectedItem.Location_Id = loc.Location_Id;
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
        private async void On_Edit(object parameter)
        {
            if (IsSelected)
            {
                var mapper = MapperConfig.InitializeAutomapper();
                ProductDto proxy = mapper.Map<ProductDto>(SelectedItem);
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
        private async void On_OpenMap(object parameter)
        {
            if (IsSelected)
            {
                if (SelectedItem.Location != null)
                    await NavigateToBuilding25(mapper.Map<Microsoft.Maui.Devices.Sensors.Location>(SelectedItem.Location));
                else
                    await Shell.Current.DisplayAlert("Message", "Get location", "Cancel");
            }
            else
                await _messageBox.ShowNotification("Please select the item first");
        }
        private async void On_Delete(object parameter)
        {
            if(IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    await _productRepository.DeleteItem(mapper.Map<Product>(SelectedItem));
                    // Update Stat
                    PurchaseStatistics purchaseStatistics = await _statisticsDB.GetItemById(SelectedItem.PurchaseId);
                    await _statisticsDB.SaveOrUpdateItem(purchaseStatistics);
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
        private async void On_Open(object parameter)
        {
            await Task.Delay(1);
        }
        private async void On_DoubleClick(object parameter)
        {
            if (IsSelected)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "details", SelectedItem }
                        };
                await Shell.Current.GoToAsync(nameof(PurchaseItemDetails), navigationParameter);
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
                    p.PurchaseId = purchase.Purchase_Id;
                    var x = mapper.Map<Product>(p);
                    await _productRepository.SaveOrUpdateItem(x);
                }
            });
        }
        private void CommandSetup()
        {
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
            DeleteCommand = new Command(On_Delete);
            OpenMapCommand = new Command(On_OpenMap);
            EditCommand = new Command(On_Edit);
            GetMapCommand = new Command(On_GetMap);
            OpenAnalyticCommand = new Command(OnOpenAnalyticCommand);
            ExportToPdfCommand = new Command(OnExportToPdfCommand);
        }
        private async void UpdateUI()
        {
            var purchase = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(purchase));
        }
        private async Task NavigateToBuilding25(Microsoft.Maui.Devices.Sensors.Location location)
        {
            try
            {
                await Map.Default.OpenAsync(location);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
       
        private async Task<Microsoft.Maui.Devices.Sensors.Location> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

                Microsoft.Maui.Devices.Sensors.Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                return location;
            }

            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
            
        }
        #endregion
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                Purchases = query["purchase"] as PurchasesDTO;
            }
        }

        public override async Task LoadItems()
        {
            ShowActivity();
            await Task.Delay(1);
            SetItems(Purchases.Products);
            HideActivity();
        }
        
    }
}
