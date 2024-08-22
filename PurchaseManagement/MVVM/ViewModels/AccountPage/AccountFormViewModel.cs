using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountFormViewModel:BaseViewModel
    {
        private double _money;
        public double Money
        {
            get => _money;
            set => UpdateObservable(ref _money, value);
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value);
        }
        #region Commands
        public ICommand AddCommand { get; private set; }

        #endregion
        public AccountFormViewModel()
        {
            SelectedDate = DateTime.Now;
            CommandSetup();
        }
        private void CommandSetup()
        {
            AddCommand = new Command(OnAdd);

        }
        #region Handlers

        private void OnAdd(object parameter)
        {
            ViewModelLocator.AccountListViewViewModel.AddItem(new AccountDTO(SelectedDate, Money));
        }
        #endregion
    }
}
