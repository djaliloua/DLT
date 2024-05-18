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
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        private Mapper mapper;
        bool CanOpen => Selected_Purchase_Item != null;
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        public PurchaseItemsViewModel(IRepository db)
        {
            _db = db;
            mapper = MapperConfig.InitializeAutomapper();
            Purchase_Items = new ObservableCollection<Purchase_ItemsDTO>();
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
            DeleteCommand = new Command(On_Delete);
            OpenMapCommand = new Command(On_OpenMap);
            EditCommand = new Command(On_Edit);
            GetMapCommand = new Command(On_GetMap);
        }
        private async Task _savePurchaseItemAndStatDb(Purchases purchase)
        {
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(Selected_Purchase_Item);
            purchase.Purchase_Items.Add(m_purchase_item);
            await _db.SavePurchaseAsync(purchase);


            // Update UI
            purchase = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
            UpdateById(mapper.Map<PurchasesDTO>(purchase));
        }
        private void UpdateById(PurchasesDTO newObj)
        {
            
            foreach(PurchasesDTO p in ViewModelLocator.MainViewModel.Purchases)
            {
                if(p.Purchase_Id == newObj.Purchase_Id)
                {
                    p.Purchase_Items = newObj.Purchase_Items;
                    p.PurchaseStatistics = newObj.PurchaseStatistics;
                    break;
                }
            }
        }
        private async void On_GetMap(object parameter)
        {
            
            ShowProgressBar();
            if (CanOpen)
            {
                Location location = await GetCurrentLocation();
                if (await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases purchases)
                {
                    var loc = mapper.Map<MarketLocation>(location);
                    Selected_Purchase_Item.Location = mapper.Map<MarketLocationDTO>(loc);
                    loc.Purchase_Id = Selected_Purchase_Item.Purchase_Id;
                    loc.Purchase_Item_Id = Selected_Purchase_Item.Item_Id;
                    Selected_Purchase_Item.IsLocation = Selected_Purchase_Item.Location != null;
                    await _db.SaveAndUpdateLocationAsync(loc);
                    
                    var purchase = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    UpdateById(mapper.Map<PurchasesDTO>(purchase));
                }
                
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
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    await _db.DeletePurchaseItemAsync(mapper.Map<Purchase_Items>(Selected_Purchase_Item));
                    var purchase = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    UpdateById(mapper.Map<PurchasesDTO>(purchase));
                    await LoadPurchaseItemsDTOAsync(mapper.Map<PurchasesDTO>(purchase).Purchase_Items);
                }
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !Show;
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
        public async Task LoadPurchaseItemsDTOAsync(IList<Purchase_ItemsDTO> items)
        {
            ShowProgressBar();
            await Task.Delay(1);
            Purchase_Items.Clear();
            var data = items.OrderByDescending(p => p.Item_Id).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                Purchase_Items.Add(mapper.Map<Purchase_ItemsDTO>(data[i]));
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
