using CommunityToolkit.Maui.Core;
using MVVM;
using CommunityToolkit.Maui.Alerts;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using AutoMapper;
using PurchaseManagement.Commons;
using PurchaseManagement.DataAccessLayer.Repository;
using Location = PurchaseManagement.MVVM.Models.Location;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IGenericRepository<Purchase> _purchaseDB;
        private readonly IGenericRepository<Product> _productDB;
        private readonly IGenericRepository<Location> _locationDB;
        private ProductDto _purchaseItem;
        public ProductDto PurchaseItem
        {
            get => _purchaseItem;
            set => UpdateObservable(ref _purchaseItem, value);
        }
        private bool _isSave;
        public bool IsSave
        {
            get => _isSave;
            set => UpdateObservable(ref _isSave, value);    
        }
        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        public int Counter = 0;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        INotication notication;

        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public MarketFormViewModel(IGenericRepository<Purchase> purchaseDB, 
            IGenericRepository<Product> productDB, 
            IGenericRepository<Location> locationDB)
        {
            _purchaseDB = purchaseDB;
            _productDB = productDB;
            _locationDB = locationDB;
            CommandSetup();
            notication = new ToastNotification();
        }
        #endregion
        private void CommandSetup()
        {
            CancelCommand = new Command(On_Cancel);
            SaveCommand = new Command(On_Save);
            BackCommand = new Command(On_Back);
            UpdateCommand = new Command(On_Update);
        }
        
        private async void On_Back(object parameter)
        {
            Counter = 0;
            await Shell.Current.GoToAsync("..");
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !Show;
        }
        
        private async void On_Update(object parameter)
        {

            if (ViewModelLocator.PurchaseItemsViewModel.IsSelected)
            {
                await _update(mapper.Map<Purchase>(ViewModelLocator.PurchaseItemsViewModel.Purchases));
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            ShowProgressBar();
            Product product = mapper.Map<Product>(PurchaseItem);
            if (await _purchaseDB.GetByDate($"{ViewModelLocator.MainViewModel.SelectedDate:yyyy-MM-dd}") is Purchase purchase)
            {
                product.PurchaseId = purchase.Id;
                product = await _productDB.SaveOrUpdateItem(product, true);
            }
            else
            {
                Purchase purchaseNew = new Purchase("test", ViewModelLocator.MainViewModel.SelectedDate);
                purchaseNew.Products.Add(product);
                product.Purchase = purchaseNew;
                product = await _productDB.SaveOrUpdateItem(product, true);
            }


            Counter++;
            notication.ShowNotification($"{Counter}");
            HideProgressBar();
        }
        private async Task _update(Purchase purchase)
        {
            
        }
       
        
        
        private void Update(PurchaseDto newObj)
        {
            ViewModelLocator.MainViewModel.UpdateItem(newObj);
        }
        
        private async void On_Cancel(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count() > 0)
            {
                IsSave = (bool)query["IsSave"];
                PurchaseItem = query["Purchase_ItemsDTO"] as ProductDto;
                Counter = PurchaseItem.Counter;
            }
        }
    }
}
