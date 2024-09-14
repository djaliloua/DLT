using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Commons.Notifications.Implementations;
using PurchaseManagement.Validations;
using PurchaseManagement.Utilities;

namespace PurchaseManagement.MVVM.ViewModels.PurchasePage
{
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        #region Public Properties
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value, () =>
            {
                ViewModelLocator.MainViewModel.DateTime = value;
            });
        }
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
        public MarketFormViewModel()
        {
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
                if(!await MarketFormViewModelUtility.UpdateProductItem(ViewModelLocator.MainViewModel.GetItemByDate(), 
                    ProductItem))
                {
                    return;
                }
            }
            await Shell.Current.GoToAsync("..");
            await _toastNotification.ShowNotification($"{ViewModelLocator.ProductItemsViewModel.SelectedItem.Item_Name} updated");
        }
        private async void OnSave(object sender)
        {
            await MarketFormViewModelUtility.CreateAndAddProduct(ProductItem);
        }
        #endregion

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count() > 0)
            {
                IsSave = (bool)query["IsSave"];
                ProductItem = query["Purchase_ItemsDTO"] as ProductDto;
                SelectedDate = IsSave ? (DateTime)query["currentDate"] : DateTime.Parse(query["currentDate"] as string);
                Counter = ProductItem.Counter;
            }
        }
    }
}
