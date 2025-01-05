using PurchaseManagement.MVVM.Models.ViewModel;
using System.Windows.Input;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Commons.Notifications.Implementations;
using Patterns.Implementations;
using Patterns.Abstractions;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.ExtensionMethods;
using Models.Account;
using Repository;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class LoadAccountService : ILoadService<AccountViewModel>
    {
        public IList<AccountViewModel> Reorder(IList<AccountViewModel> items)
        {
            return items.OrderByDescending(a => a.DateTime).ToList();
        }
    }
    public class AccountListViewViewModel : Loadable<AccountViewModel>
    {
        #region Private methods
        private INotification _snackBarNotification;
        private INotification _toastNotification;
        private INotification _messageBox;
        #endregion

        #region Properties
        private MaxMin _maxSaleValue;
        public MaxMin MaxSaleValue
        {
            get => _maxSaleValue;
            set => UpdateObservable(ref _maxSaleValue, value);
        }
        
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        #endregion

        #region Constructor
        public AccountListViewViewModel(ILoadService<AccountViewModel> loadService):base(loadService)
        {
            SetupNotification();
            Init();
            SetupComands();
        }
        #endregion

        #region Private methods
        public override bool ItemExist(AccountViewModel newAccount)
        {
            foreach (AccountViewModel account in Items)
            {
                if ($"{account.DateTime:yyyy-MM-dd}".Equals($"{newAccount.DateTime:yyyy-MM-dd}"))
                    return true;
            }
            return false;
        }

        private void SetupNotification()
        {
            _snackBarNotification = new SnackBarNotification();
            _messageBox = new MessageBoxNotification();
            _toastNotification = new ToastNotification();
        }
        private async void Init()
        {
            ShowActivity();
            await Task.Run(async () =>
            {
                using var repo = new AccountRepository();
                IList<Account> data = await repo.GetAllItemsAsync();
                await LoadItems(data.ToVM<Account, AccountViewModel>());
                await GetMax()
                    .ContinueWith(async (t) =>
                    {
                        if (!IsEmpty)
                        {
                            await _snackBarNotification.ShowNotification($"High expense: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value:C2}");
                        }
                    }
                    );
            });
            HideActivity();
        }
        private void SetupComands()
        {
            DeleteCommand = new Command(OnDelete);
            RefreshCommand = new Command(OnRefresh);
            EditCommand = new Command(OnEdit);
        }
        public async void OnEdit(object parameter)
        {
            await _toastNotification.ShowNotification("Not yet implemented");
        }
        private async Task GetMax()
        {
            MaxMin max = new();
            using var repo = new AccountRepository();
            IList<MaxMin> val = await repo.GetMaxAsync();
            if (val.Count == 1)
            {
                max = val[0];
            }
            MaxSaleValue = max;
        }
        #endregion

        #region Overriden methods
        
        #endregion

        #region Handlers
        private void OnRefresh(object parameter)
        {
            IsRefreshed = true;
            Init();
            IsRefreshed = false;
        }
        private void OnDelete(object parameter)
        {
            AccountViewModel accountDTO = parameter as AccountViewModel;
            SelectedItem = accountDTO;
            DeleteItem(SelectedItem);
        }
        public override async void AddItem(AccountViewModel item)
        {
            
            if (!ItemExist(item))
            {
                using var repo = new AccountRepository();
                var newAccount = repo.Save(item);
                base.AddItem(newAccount);
                await _toastNotification.ShowNotification($"{newAccount.Money} added");
                ViewModelLocator.AccountFormViewModel.Money = 0;
            }
            else
            {
                await _messageBox.ShowNotification("Item already present");
            }
        }
        
        public async override void DeleteItem(AccountViewModel item)
        {
            
            if (IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    var acount = item;
                    using var repo = new AccountRepository();
                    await repo.DeleteAsync(acount.Id);
                    base.DeleteItem(item);
                    await _toastNotification.ShowNotification($"{item.Money} deleted");
                }
            }
            else
            {
                await _messageBox.ShowNotification("Please select the item first");
            }
        }
        
        #endregion
    }
    
}
