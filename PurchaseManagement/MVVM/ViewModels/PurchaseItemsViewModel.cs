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
    public abstract class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: Purchase_ItemsDTO
    {
        private PurchasesDTO _purchases;
        public PurchasesDTO Purchases
        {
            get => _purchases;
            set
            {
                _purchases = value;
                SetItems((IList<TItem>)value.Purchase_Items);
            }
        }
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(item => item.Item_Id).ToList();
            SetItems(data);
        }

    }
    public class PurchaseItemsViewModel: PurchaseItemsViewModelLoadable<Purchase_ItemsDTO>, IQueryAttributable
    {
        private readonly IRepository _db;
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
        public PurchaseItemsViewModel(IRepository db)
        {
            _db = db;
            _ = LoadItems();
            CommandSetup();
            WeakReferenceMessenger.Default.Register<Purchase_ItemsDTO, string>(this,"update", async (sender, p) =>
            {
                if(p.Purchase is PurchasesDTO purchase)
                    await db.SavePurchaseItemAsync(mapper.Map<Purchase_Items>(p));
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
                Location location = await GetCurrentLocation();
                if (await _db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases)
                {
                    var loc = mapper.Map<MarketLocation>(location);
                    SelectedItem.Location = mapper.Map<MarketLocationDTO>(loc);
                    loc.Purchase_Id = SelectedItem.Purchase_Id;
                    loc.Purchase_Item_Id = SelectedItem.Item_Id;
                    SelectedItem.IsLocation = SelectedItem.Location != null;
                    await _db.SaveAndUpdateLocationAsync(loc);
                    
                    var purchase = await _db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(purchase));
                }
                
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
                Purchase_ItemsDTO proxy = mapper.Map<Purchase_ItemsDTO>(SelectedItem);
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
                if (SelectedItem.Location != null)
                    await NavigateToBuilding25(mapper.Map<Location>(SelectedItem.Location));
                else
                    await Shell.Current.DisplayAlert("Message", "Get location", "Cancel");
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
                    await _db.DeletePurchaseItemAsync(mapper.Map<Purchase_Items>(SelectedItem));
                    var p = await _db.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);

                    ViewModelLocator.MainViewModel.UpdateItem(mapper.Map<PurchasesDTO>(p));
                    DeleteItem(SelectedItem);
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
            if (IsSelected)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "details", SelectedItem }
                        };
                await Shell.Current.GoToAsync(nameof(PurchaseItemDetails), navigationParameter);
            }
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

        public override async Task LoadItems()
        {
            ShowActivity();
            await Task.Delay(1);
            //SetItems(Purchases.Purchase_Items);
            HideActivity();
        }
        
    }
}
