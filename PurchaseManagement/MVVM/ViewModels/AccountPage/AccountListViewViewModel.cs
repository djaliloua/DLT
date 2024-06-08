using AutoMapper;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using System.Windows.Input;
using Patterns;

/* Unmerged change from project 'PurchaseManagement (net8.0)'
Before:
using PurchaseManagement.ServiceLocator;
After:
using PurchaseManagement.ServiceLocator;
using PurchaseManagement;
using PurchaseManagement.MVVM;
using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
*/

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountListViewViewModel : Loadable<AccountDTO>
    {
        #region Private methods
        private readonly IAccountRepository accountRepository;
        private readonly IAccountRepositoryAPI accountRepositoryAPI;
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
        public AccountListViewViewModel(IAccountRepository _accountRepository, IAccountRepositoryAPI _accountRepositoryAPI)
        {
            accountRepository = _accountRepository;
            accountRepositoryAPI = _accountRepositoryAPI;
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

        private async Task MakeSnackBarAsync(string msg)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Black,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                CharacterSpacing = 0.5
            };
            string text = msg;
            TimeSpan duration = TimeSpan.FromSeconds(5);
            var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }
        private async void Init()
        {
            await LoadItems();
            await GetMax()
                .ContinueWith(async (t) =>
                {
                    if (!IsEmpty)
                    {
                        await MakeSnackBarAsync($"Best day: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value} CFA");
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
            var data = await accountRepositoryAPI.GetAccounts();
            var dt = data.Select(mapper.Map<AccountDTO>).OrderByDescending(a => a.DateTime).ToList();
            SetItems(dt);
            HideActivity();
        }
        public override async void AddItem(AccountDTO account)
        {
            if (!ItemExist(account))
            {
                var y = await accountRepositoryAPI.PostAccount(mapper.Map<Account>(account));
                var x = await accountRepository.SaveOrUpdateAsync(mapper.Map<Account>(account));
                base.AddItem(mapper.Map<AccountDTO>(x));
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Item already present", "Cancel");
            }
        }
        public override async void DeleteItem(AccountDTO account)
        {
            if (IsSelected)
            {
                if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    var acount = mapper.Map<Account>(account);
                    await accountRepository.DeleteAsync(acount);
                    await accountRepositoryAPI.DeleteAccount(acount.Id);
                    base.DeleteItem(account);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            }
        }
        #endregion

        #region Handlers

        private void On_Delete(object parameter)
        {
            DeleteItem(SelectedItem);
        }

        public async void AddAccount(AccountDTO account)
        {
            
        }

        

        #endregion
    }
    
}
