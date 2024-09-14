using MVVM;
using System.Windows.Input;
using PurchaseManagement.Commons;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Commons.Notifications.Implementations;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountPageViewModel : BaseViewModel
    {
        #region Private methods
        private INotification toastNotification;
        private ExportContext<AccountDTO> exportContext;
        #endregion

        #region Properties

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
        public ICommand ExportToPdfCommand { get; private set; }
        #endregion

        #region Constructor
        public AccountPageViewModel(ExportContext<AccountDTO> context)
        {
            AccountListViewViewModel = ViewModelLocator.AccountListViewViewModel;
            AccountHeaderViewModel = ViewModelLocator.AccountHeaderViewModel;
            toastNotification = new ToastNotification();
            exportContext = context;
            //Init();
            CommandSetup();
        }
        #endregion

        #region Private Methods

        private void CommandSetup()
        {
            ExportToPdfCommand = new Command(OnExportToPdfCommand);
        }
        private async void OnExportToPdfCommand(object parameter)
        {
            await Task.Delay(1);
            exportContext.ExportTo("", AccountListViewViewModel.GetItems());
        }
        //private void Init()
        //{
        //    SelectedDate = DateTime.Now;
        //    IsSavebtnEnabled = true;
        //}

        #endregion

        #region Handlers
        //private async void OnAdd(object parameter)
        //{
        //    await Shell.Current.GoToAsync(nameof(AccountForm));
        //}
        #endregion
    }
}
