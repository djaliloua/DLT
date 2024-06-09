using AutoMapper;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.DTOs;
using System.Windows.Input;
using Patterns;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.Commons;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public interface IAccountListViewMethods
    {
        void AddAccount(AccountDTO account);
        void DeleteAccount(AccountDTO account);
    }
    public class AccountListViewViewModel : Loadable<AccountDTO>, IAccountListViewMethods
    {
        #region Private methods
        private readonly IAccountRepository accountRepository;
        private readonly INotification _notification;
        private Mapper mapper = MapperConfig.InitializeAutomapper();
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
        public AccountListViewViewModel(IAccountRepository _accountRepository, INotification notification)
        {
            accountRepository = _accountRepository;
            _notification = notification;
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

        private async void Init()
        {
            await LoadItems();
            await GetMax()
                .ContinueWith( (t) =>
                {
                    if (!IsEmpty)
                    {
                        _notification.ShowNotification($"Best day: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value} CFA");
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
            var data = await accountRepository.GetAllItems();
            var dt = data.Select(mapper.Map<AccountDTO>).OrderByDescending(a => a.DateTime).ToList();
            SetItems(dt);
            HideActivity();
        }
        #endregion

        #region Handlers

        private void On_Delete(object parameter)
        {
            DeleteAccount(SelectedItem);
        }

        public async void AddAccount(AccountDTO account)
        {
            if (!ItemExist(account))
            {
                //var y = await accountRepositoryAPI.PostAccount(mapper.Map<Account>(account));
                var newAccount = await accountRepository.SaveOrUpdateItem(mapper.Map<Account>(account));
                AddItem(mapper.Map<AccountDTO>(newAccount));
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Item already present", "Cancel");
            }
        }

        public async void DeleteAccount(AccountDTO account)
        {
            if (IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    var acount = mapper.Map<Account>(account);
                    await accountRepository.DeleteItem(acount);
                    //await accountRepositoryAPI.DeleteAccount(acount.Id);
                    DeleteItem(account);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            }
        }



        #endregion
    }
    
}
