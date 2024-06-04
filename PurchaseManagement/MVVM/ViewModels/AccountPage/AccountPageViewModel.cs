using MVVM;
using System.Windows.Input;
using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountPageViewModel : BaseViewModel
    {
        #region Private methods
        #endregion

        #region Properties
        //private long _money;
        //public long Money
        //{
        //    get => _money;
        //    set => UpdateObservable(ref _money, value);
        //}


        //private bool _isSavebtnEnabled;
        //public bool IsSavebtnEnabled
        //{
        //    get => _isSavebtnEnabled;
        //    set => UpdateObservable(ref _isSavebtnEnabled, value);
        //}
        //private DateTime _selectedDate;
        //public DateTime SelectedDate
        //{
        //    get => _selectedDate;
        //    set => UpdateObservable(ref _selectedDate, value);
        //}
        private AccountListViewViewModel _accountListViewModel;
        public AccountListViewViewModel AccountListViewViewModel
        {
            get => _accountListViewModel;
            set => UpdateObservable(ref _accountListViewModel, value);
        }
        private AccountHeaderViewModel _accountHeaderViewModel;
        public AccountHeaderViewModel AccountHeaderViewModel
        {
            get => _accountHeaderViewModel;
            set => UpdateObservable(ref _accountHeaderViewModel, value);
        }
        #endregion

        #region Commands
        //public ICommand AddCommand { get; private set; }
        #endregion

        #region Constructor
        public AccountPageViewModel()
        {
            AccountListViewViewModel = ViewModelLocator.AccountListViewViewModel;
            AccountHeaderViewModel = ViewModelLocator.AccountHeaderViewModel;
            //Init();
            //CommandSetup();
        }
        #endregion

        #region Private Methods

        //private void CommandSetup()
        //{
        //    AddCommand = new Command(OnAdd);
        //}
        //private void Init()
        //{
        //    SelectedDate = DateTime.Now;
        //    IsSavebtnEnabled = true;
        //}

        #endregion

        #region Handlers
        //private void OnAdd(object parameter)
        //{
        //    AccountListViewViewModel.AddAccount(new AccountDTO(SelectedDate, Money));
        //}
        #endregion
    }
}
