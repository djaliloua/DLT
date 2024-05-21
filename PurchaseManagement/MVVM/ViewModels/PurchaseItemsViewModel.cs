﻿using AutoMapper;
using CommunityToolkit.Maui.Core.Extensions;
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
    public abstract class PurchaseItemsViewModelLoadable<TItem>: Loadable<TItem> where TItem: Purchase_ItemsDTO
    {
        
        public double GetTotalValue(string colname)
        {
            double result = 0;
            if (colname == "Price")
                result = GetItems().Sum(x => x.Item_Price);
            else
                result = GetItems().Sum(x => x.Item_Quantity);
            return result;
        }
        public override void Reorder()
        {
            var data = Items.OrderByDescending(item => item.Item_Id).ToList();
            SetItems(data);
        }
        public int CountPurchaseItems() => Counter;

    }
    public class PurchaseItemsViewModel: PurchaseItemsViewModelLoadable<Purchase_ItemsDTO>, IQueryAttributable
    {
        private readonly IRepository _db;

        public PurchasesDTO Purchases;
        
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
        public ICommand DoubleClickCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand GetMapCommand { get; private set; }
        public PurchaseItemsViewModel(IRepository db)
        {
            _db = db;
            _ = LoadItems();
            DoubleClickCommand = new Command(On_DoubleClick);
            OpenCommand = new Command(On_Open);
            DeleteCommand = new Command(On_Delete);
            OpenMapCommand = new Command(On_OpenMap);
            EditCommand = new Command(On_Edit);
            GetMapCommand = new Command(On_GetMap);
        }
        private async Task _savePurchaseItemAndStatDb(Purchases purchase)
        {
            Purchase_Items m_purchase_item = mapper.Map<Purchase_Items>(SelectedItem);
            purchase.Purchase_Items.Add(m_purchase_item);
            await _db.SavePurchaseAsync(purchase);

            // Update UI
            purchase = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
            ViewModelLocator.MainViewModel.Update(mapper.Map<PurchasesDTO>(purchase));
        }
        private void UpdateById(PurchasesDTO newObj)
        {
            foreach(PurchasesDTO p in ViewModelLocator.MainViewModel.GetItems())
            {
                if(p.Purchase_Id == newObj.Purchase_Id)
                {
                    p.Purchase_Items = newObj.Purchase_Items;
                    p.PurchaseStatistics = newObj.PurchaseStatistics;
                    break;
                }
            }
            ViewModelLocator.MainViewModel.Reorder();
        }
        private async void On_GetMap(object parameter)
        {
            ShowProgressBar();
            if (IsSelected)
            {
                Location location = await GetCurrentLocation();
                if (await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchases)
                {
                    var loc = mapper.Map<MarketLocation>(location);
                    SelectedItem.Location = mapper.Map<MarketLocationDTO>(loc);
                    loc.Purchase_Id = SelectedItem.Purchase_Id;
                    loc.Purchase_Item_Id = SelectedItem.Item_Id;
                    SelectedItem.IsLocation = SelectedItem.Location != null;
                    await _db.SaveAndUpdateLocationAsync(loc);
                    
                    var purchase = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);
                    ViewModelLocator.MainViewModel.Update(mapper.Map<PurchasesDTO>(purchase));
                }
                
            }
            else
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            HideProgressBar();
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
                    var p = await _db.GetPurchasesByDate(ViewModelLocator.MainViewModel.SelectedDate);

                    ViewModelLocator.MainViewModel.Update(mapper.Map<PurchasesDTO>(p));
                    DeleteItem(SelectedItem);
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
            ShowProgressBar();
            await Task.Delay(1);
            SetItems(Purchases.Purchase_Items);
            HideProgressBar();
        }
        
    }
}
