using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.DTOs;
using System.Windows.Input;
using Patterns;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.Commons;
using Mapster;
using MapsterMapper;
using PurchaseManagement.Commons.Notifications;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    
    public class AccountListViewViewModel : Loadable<AccountDTO>
    {
        #region Private methods
        private readonly IAccountRepository accountRepository;
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

        public ICommand DeleteCommand { get; private set; }
        #endregion

        #region Constructor
        public AccountListViewViewModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
            SetupNotification();
            Init();
            SetupComands();
        }
#endregion

        #region Private methods
        public override bool ItemExist(AccountDTO newAccount)
        {
            foreach (AccountDTO account in Items)
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
            await LoadItems();
            await GetMax()
                .ContinueWith(async (t) =>
                {
                    if (!IsEmpty)
                    {
                        await _snackBarNotification.ShowNotification($"Best day: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value} CFA");
                    }
                }
                );
        }
        private void SetupComands()
        {
            DeleteCommand = new Command(On_Delete);
        }
        private async Task GetMax()
        {
            MaxMin max = new();
            IList<MaxMin> val = await accountRepository.GetMaxAsync();
            if (val.Count == 1)
            {
                max = val[0];
            }
            MaxSaleValue = max;
        }
        #endregion

        #region Overriden methods
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(a => a.DateTime).ToList();
            SetItems(data);
        }
        public override async Task LoadItems()
        {
            ShowActivity();
            IEnumerable<Account> data =  await accountRepository.GetAllItemsAsync();
            var dt = data.Adapt<List<AccountDTO>>();
            SetItems(dt);
            HideActivity();

        }
        #endregion

        #region Handlers

        private void On_Delete(object parameter)
        {
            AccountDTO accountDTO = parameter as AccountDTO;
            SelectedItem = accountDTO;
            DeleteItem(SelectedItem);
        }
        public override async void AddItem(AccountDTO item)
        {
            
            if (!ItemExist(item))
            {
                var newAccount = await accountRepository.SaveOrUpdateItemAsync(item.Adapt<Account>());
                base.AddItem(newAccount.Adapt<AccountDTO>());
                await _toastNotification.ShowNotification($"{newAccount.Money} added");
            }
            else
            {
                await _messageBox.ShowNotification("Item already present");
            }
        }
        
        public async override void DeleteItem(AccountDTO item)
        {
            
            if (IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    var acount = item.Adapt<Account>();
                    await accountRepository.DeleteItemAsync(acount);
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
