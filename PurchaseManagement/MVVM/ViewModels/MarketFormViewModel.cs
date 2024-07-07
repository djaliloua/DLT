using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.Validations;
using FluentValidation.Results;
using PurchaseManagement.Utilities;
using PurchaseManagement.Commons.Notifications;
using Mapster;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        #region Public Properties
        private ProductDto _purchaseItem;
        public ProductDto ProductItem
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
        #endregion

        #region Private Methods
        public int Counter = 0;
        private readonly IPurchaseRepository _purchaseDB;
        private readonly INotification _toastNotification;
        private ProductValidation productValidation = new();
        #endregion

        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public MarketFormViewModel(IPurchaseRepository db)
        {
            _purchaseDB = db;
            _toastNotification = new ToastNotification();
            IsSavebtnEnabled = true;
            CommandSetup();
        }
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave);
            BackCommand = new Command(OnBack);
            UpdateCommand = new Command(OnUpdate);
        }
        #endregion

        #region Handlers
        private async void OnCancel(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnBack(object parameter)
        {
            Counter = 0;
            await Shell.Current.GoToAsync("..");
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !IsActivity;
        }
        
        private async void OnUpdate(object parameter)
        {
            if (ViewModelLocator.ProductItemsViewModel.IsSelected)
            {
                if(!await UpdateProductItem(await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate)))
                {
                    return;
                }
            }
            await Shell.Current.GoToAsync("..");
            await _toastNotification.ShowNotification($"{ViewModelLocator.ProductItemsViewModel.SelectedItem.Item_Name} updated");
        }
        private async void OnSave(object sender)
        {
            ValidationResult validationResult = productValidation.Validate(ProductItem);
            if (validationResult.IsValid)
            {
                ShowActivity();
                //ViewModelLocator.MainViewModel.GetItemByDate() is PurchaseDto purchaseDto
                if (await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchase purchase)
                {
                    AddNewProducts(purchase);
                }
                else
                {
                    SavePurchaseAndProductItem(new Purchase("test", ViewModelLocator.MainViewModel.SelectedDate));
                }
                Counter++;
                await _toastNotification.ShowNotification($"{Counter}");
                HideActivity();
            }
            else
            {
                await _toastNotification.ShowNotification(validationResult.Errors[0].ErrorMessage);
            }
        }
        #endregion

        #region Private Methods
        private async Task<bool> UpdateProductItem(Purchase purchase)
        {
            ValidationResult validationResult = productValidation.Validate(ProductItem);
            if(validationResult.IsValid)
            {
                Product m_purchase_item = ProductItem.Adapt<Product>();
                updateProduct(purchase, m_purchase_item);
                
                // Update UI
                await SaveAndUpdateUI(purchase);
            }
            else
            {
                await _toastNotification.ShowNotification(validationResult.Errors[0].ErrorMessage);
                return false;
            }
            return true;
            
        }
        private void updateProduct(Purchase purchase, Product product)
        {
            for (int i = 0; i < purchase.Products.Count; i++)
            {
                if (purchase.Products[i].Id == product.Id)
                    purchase.Products[i] = product;
            }
            purchase.UpdateStatistics();
        }
        private async void AddNewProducts(Purchase purchase)
        {
            // Update DB
            Product m_product_item = ProductItem.Adapt<Product>();
            purchase.Add(m_product_item);

            //
            await SaveAndUpdateUI(purchase);
        }
        private async void UpdateUI()
        {
            var purchase = await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            Update(purchase.Adapt<PurchaseDto>());
        }
        private void Update(PurchaseDto newObj)
        {
            ViewModelLocator.MainViewModel.UpdateItem(newObj);
        }
        private async Task SaveAndUpdateUI(Purchase purchase)
        {
            Purchase purchaseB = await _purchaseDB.SaveOrUpdateItemAsync(purchase);
            PurchaseDto p = purchaseB.Adapt<PurchaseDto>();
            ViewModelLocator.MainViewModel.SaveOrUpdateItem(p);
        }
        private async void SavePurchaseAndProductItem(Purchase purchase)
        {
            purchase.Add(ProductItem.Adapt<Product>());
            await SaveAndUpdateUI(purchase);
        }
        
        #endregion
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count() > 0)
            {
                IsSave = (bool)query["IsSave"];
                ProductItem = query["Purchase_ItemsDTO"] as ProductDto;
                Counter = ProductItem.Counter;
            }
        }
    }
}
