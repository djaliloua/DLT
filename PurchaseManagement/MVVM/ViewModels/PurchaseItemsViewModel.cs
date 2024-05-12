using AutoMapper;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Pages;
using PurchaseManagement.ServiceLocator;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemsViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository _db;
        public ObservableCollection<Purchase_ItemsDTO> Purchase_Items { get; }
        public PurchasesDTO Purchases;
        private Purchase_ItemsDTO _selected_Purchase_Item;
        public Purchase_ItemsDTO Selected_Purchase_Item
        {
            get => _selected_Purchase_Item;
            set => UpdateObservable(ref _selected_Purchase_Item, value);
        }
        private bool _isLocAvailable;
        public bool IsLocAvailable
        {
            get => _isLocAvailable;
            set => UpdateObservable(ref _isLocAvailable, value);
        }
        private Mapper mapper;
        bool CanOpen => Selected_Purchase_Item != null;
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        public PurchaseItemsViewModel(IRepository _db)
        {
            this._db = _db;
            mapper = MapperConfig.InitializeAutomapper();
            Purchase_Items = new ObservableCollection<Purchase_ItemsDTO>();
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
            DeleteCommand = new Command(On_Delete);
            OpenMapCommand = new Command(On_OpenMap);
            EditCommand = new Command(On_Edit);
            GetMapCommand = new Command(On_GetMap);
        }
        private async void On_GetMap(object parameter)
        {
            ShowProgressBar();
            if (CanOpen)
            {
                Location location = await GetCurrentLocation();;
                if (await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
                {
                    var loc = mapper.Map<MarketLocation>(location);
                    loc.Purchase_Id = Selected_Purchase_Item.Purchase_Id;
                    loc.Purchase_Item_Id = Selected_Purchase_Item.Item_Id;
                    await _db.SaveAndUpdateLocationAsync(loc);
                    await _db.SavePurchaseItemAsync(mapper.Map<Purchase_Items>(Selected_Purchase_Item));
                }
                await LoadPurchaseItemsAsync(Selected_Purchase_Item.Purchase_Id);


            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            HideProgressBar();
        }
        private async void On_Edit(object parameter)
        {
            if (CanOpen)
            {
                var mapper = MapperConfig.InitializeAutomapper();
                Purchase_ItemsDTO proxy = mapper.Map<Purchase_ItemsDTO>(Selected_Purchase_Item);
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
            if (CanOpen)
            {
                if (Selected_Purchase_Item.Location != null)
                    await NavigateToBuilding25(mapper.Map<Location>(Selected_Purchase_Item.Location));
                else
                    await Shell.Current.DisplayAlert("Message", "Get location", "Cancel");
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }
        private async void On_Delete(object parameter)
        {
            if(CanOpen)
            {
                if(await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    await _db.DeletePurchaseItemAsync(mapper.Map<Purchase_Items>(Selected_Purchase_Item));
                    await LoadPurchaseItemsAsync(Purchases.Purchase_Id);
                    Purchases.PurchaseStatistics.PurchaseCount = await _db.CountPurchaseItems(Purchases.Purchase_Id); ;
                    Purchases.PurchaseStatistics.TotalPrice = await _db.GetTotalValue(mapper.Map<Purchases>(Purchases), "price");
                    Purchases.PurchaseStatistics.TotalQuantity = await _db.GetTotalValue(mapper.Map<Purchases>(Purchases), "quantity");
                    await _db.SavePurchaseStatisticsItemAsyn(mapper.Map<Purchases>(Purchases).PurchaseStatistics);
                    await ViewModelLocator.MainViewModel.LoadPurchasesAsync();
                }
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }
         private async void On_Open(object parameter)
        {
            await Task.Delay(1);
        }
        private async Task NavigateToBuilding25(Location location)
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
            if (CanOpen)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "details", Selected_Purchase_Item }
                        };
                await Shell.Current.GoToAsync(nameof(PurchaseItemDetails), navigationParameter);
            }
        }
        public async Task LoadPurchaseItemsAsync(int purchaseId)
        {
            ShowProgressBar();
            await Task.Delay(1);
            Purchase_Items.Clear();
            for(int i = 0; i < Purchases.Purchase_Items.Count; i++)
            {
                Purchase_Items.Add(mapper.Map<Purchase_ItemsDTO>(Purchases.Purchase_Items[i]));
            }
            HideProgressBar();
        }
        private async Task<Location> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
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
                Purchases = query["purchase"] as PurchasesDTO;
            }
        }
    }
}
