﻿using MVVM;
using PurchaseManagement.Pages;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountHeaderViewModel:BaseViewModel
    {
        #region Private methods
        #endregion

        #region Properties
        private double _money;
        public double Money
        {
            get => _money;
            set => UpdateObservable(ref _money, value);
        }


        private bool _isSavebtnEnabled;
        public bool IsSavebtnEnabled
        {
            get => _isSavebtnEnabled;
            set => UpdateObservable(ref _isSavebtnEnabled, value);
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value);
        }

        #endregion

        #region Commands
        public ICommand AddCommand { get; private set; }
        
        #endregion

        #region Constructor
        public AccountHeaderViewModel()
        {
            Init();
            CommandSetup();
        }
        #endregion

        #region Private Methods

        private void CommandSetup()
        {
            AddCommand = new Command(OnAdd);
            
        }
        private void Init()
        {
            SelectedDate = DateTime.Now;
            IsSavebtnEnabled = true;
        }

        #endregion

        #region Handlers
        
        private async void OnAdd(object parameter)
        {
            //ViewModelLocator.AccountListViewViewModel.AddItem(new AccountDTO(SelectedDate, Money));
            //Money = 0;
            await Shell.Current.GoToAsync(nameof(AccountForm));
        }
        #endregion
    }
}
