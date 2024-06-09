using MVVM;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;
using AutoMapper;
using PurchaseManagement.DataAccessLayer.RepositoryTest;
using PurchaseManagement.Commons;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MarketFormViewModel:BaseViewModel, IQueryAttributable
    {
        #region Public Properties
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
        #endregion

        #region Private Methods
        public int Counter = 0;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        private readonly IPurchaseRepository _purchaseDB;
        private readonly IGenericRepository<PurchaseStatistics> _statisticsDB;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        #endregion

        #region Commands
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public MarketFormViewModel(IPurchaseRepository db,
            IGenericRepository<PurchaseStatistics> statisticsDB,
            INotification notification,
            IProductRepository productRepository)
        {
            _purchaseDB = db;
            _statisticsDB = statisticsDB;
            _productRepository = productRepository;
            _notification = notification;
            IsSavebtnEnabled = true;
            CommandSetup();
            
        }
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            CancelCommand = new Command(On_Cancel);
            SaveCommand = new Command(On_Save);
            BackCommand = new Command(On_Back);
            UpdateCommand = new Command(On_Update);
        }
        #endregion

        #region Handlers
        private async void On_Cancel(object sender)
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Back(object parameter)
        {
            Counter = 0;
            await Shell.Current.GoToAsync("..");
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !IsActivity;
        }
        
        private async void On_Update(object parameter)
        {

            if (ViewModelLocator.ProductItemsViewModel.IsSelected)
            {
                await UpdateProductItem(mapper.Map<Purchase>(ViewModelLocator.ProductItemsViewModel.Purchases));
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void On_Save(object sender)
        {
            ShowActivity();
            Purchase purchase = new Purchase("test", ViewModelLocator.MainViewModel.SelectedDate);
            if (await _purchaseDB.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchase purchases)
            {
                await AddNewProducts(purchases);
            }
            else
            {
                await SavePurchaseAndProductItem(purchase);
            }
            Counter++;
            _notification.ShowNotification($"{Counter}");
            HideActivity();
        }
        #endregion



        #region Private Methods
        private async Task UpdateProductItem(Purchase purchase)
        {
            Product m_purchase_item = mapper.Map<Product>(PurchaseItem);
            PurchaseStatistics stat = await _statisticsDB.GetItemById(purchase.Purchase_Id);
            purchase.PurchaseStatistics = stat;
            m_purchase_item.Purchase = purchase;
            m_purchase_item.PurchaseId = purchase.Purchase_Id;
            await _productRepository.SaveOrUpdateItem(m_purchase_item);
            var s = await _statisticsDB.SaveOrUpdateItem(stat);
            purchase.Purchase_Stats_Id = s.Id;
            _ = await _purchaseDB.SaveOrUpdateItem(purchase);

            // Update UI
            UpdateUI();
        }

        private async Task AddNewProducts(Purchase purchase)
        {
            Product m_purchase_item = mapper.Map<Product>(PurchaseItem);
            PurchaseStatistics stat = await _statisticsDB.GetItemById(purchase.Purchase_Id);
            purchase.PurchaseStatistics = stat;
            m_purchase_item.Purchase = purchase;
            m_purchase_item.PurchaseId = purchase.Purchase_Id;
            await _productRepository.SaveOrUpdateItem(m_purchase_item);
            var s = await _statisticsDB.SaveOrUpdateItem(stat);
            purchase.Purchase_Stats_Id = s.Id;
            await _purchaseDB.SaveOrUpdateItem(purchase);

            // UI
            UpdateUI();

        }
        private async void UpdateUI()
        {
            var purchase = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            Update(mapper.Map<PurchasesDTO>(purchase));
        }
        private void Update(PurchasesDTO newObj)
        {
            ViewModelLocator.MainViewModel.UpdateItem(newObj);
        }
        private async Task SavePurchaseAndProductItem(Purchase purchase)
        {
            purchase = await _purchaseDB.SaveOrUpdateItem(purchase);
            PurchaseStatistics m_purchaseStatistics = new(purchase.Purchase_Id, 1, PurchaseItem.Item_Price, PurchaseItem.Item_Quantity);
            Product m_purchase_item = mapper.Map<Product>(PurchaseItem);

            //
            await _productRepository.SaveOrUpdateItem(m_purchase_item);
            //
            m_purchase_item.PurchaseId = purchase.Purchase_Id;
            m_purchase_item.Purchase = purchase;
            m_purchaseStatistics = await _statisticsDB.SaveOrUpdateItem(m_purchaseStatistics);

            

            // Update Statistics
            purchase.PurchaseStatistics = m_purchaseStatistics;
            purchase.Purchase_Stats_Id = m_purchaseStatistics.Id;
            await _purchaseDB.SaveOrUpdateItem(purchase);
            
            //
            var p = await _purchaseDB.GetFullPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate);
            ViewModelLocator.MainViewModel.AddItem(mapper.Map<PurchasesDTO>(p));
        }
        
        #endregion
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
