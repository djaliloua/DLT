using AutoMapper;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using System.Diagnostics;
using System.Windows.Input;
using Patterns;
using CommunityToolkit.Mvvm.Messaging;

namespace PurchaseManagement.MVVM.ViewModels
{
    public abstract class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: ProductDto
    {
        private PurchaseDto _purchases;
        public PurchaseDto Purchases
        {
            get => _purchases;
            set
            {
                _purchases = value;
                SetItems((IList<TItem>)value.Products);
            }
        }
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(item => item.Id).ToList();
            SetItems(data);
        }

    }
    public class PurchaseItemsViewModel: PurchaseItemsViewModelLoadable<ProductDto>, IQueryAttributable
    {
        //private readonly IRepository _db;
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
        private readonly Mapper mapper = MapperConfig.InitializeAutomapper();
        #region Commands
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        #endregion

        #region Constructor
        public PurchaseItemsViewModel()
        {
            //_db = db;
            _ = LoadItems();
            CommandSetup();
            WeakReferenceMessenger.Default.Register<ProductDto, string>(this,"update", async (sender, p) =>
            {
                //if(p.Purchase is PurchaseDto purchase)
                //    await db.SavePurchaseItemAsync(mapper.Map<Product>(p));
            });
        }
        #endregion
        private void CommandSetup()
        {
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
            DeleteCommand = new Command(On_Delete);
            OpenMapCommand = new Command(On_OpenMap);
            EditCommand = new Command(On_Edit);
            GetMapCommand = new Command(On_GetMap);
        }
        //private async Task _savePurchaseItemAndStatDb(Purchases purchase)
        //{
        //    Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(SelectedItem);
        //    purchase.Purchase_Items.Add(m_purchase_item);
        //    await _db.SavePurchaseAsync(purchase);

        //    // Update UI
        //    purchase = await _db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
        //    ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(purchase));
        //}
        
        private async void On_GetMap(object parameter)
        {
            ShowActivity();
            if (IsSelected)
            {
                Microsoft.Maui.Devices.Sensors.Location location = await GetCurrentLocation();
                
                
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
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
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }
        private async void On_OpenMap(object parameter)
        {
            if (IsSelected)
            {
                //if (SelectedItem.Location != null)
                //    await NavigateToBuilding25(mapper.Map<Microsoft.Maui.Devices.Sensors.Location>(SelectedItem.Location));
                //else
                //    await Shell.Current.DisplayAlert("Message", "Get location", "Cancel");
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }
        private async void On_Delete(object parameter)
        {
            if(IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    //await _db.DeletePurchaseItemAsync(mapper.Map<Product>(SelectedItem));
                    //var p = await _db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);

                    //ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchaseDto>(p));
                    //DeleteItem(SelectedItem);
                }
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }

        private async void On_Open(object parameter)
        {
            await Task.Delay(1);
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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                Purchases = query["purchase"] as PurchaseDto;
            }
        }

        public override async Task LoadItems()
        {
            ShowActivity();
            await Task.Delay(1);
            //SetItems(Purchases.Purchase_Items);
            HideActivity();
        }
        
    }
}
