﻿using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AccountViewModel:BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        public ObservableCollection<Account> Accounts { get; }
        private long _money;
        public long Money
        {
            get => _money;
            set => UpdateObservable(ref _money, value);
        }
        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set => UpdateObservable(ref _selectedAccount, value);
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => UpdateObservable(ref _selectedDate, value);
        }
        
        private bool CanProceed => SelectedAccount != null;
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public AccountViewModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
            Accounts = new ObservableCollection<Account>();
            SelectedDate = DateTime.Now;
            _ = Load();
            AddCommand = new Command(On_Add);
            DeleteCommand = new Command(On_Delete);
        }
        private async void On_Delete(object parameter)
        {
            if (CanProceed)
            {
                await accountRepository.DeleteAsync(SelectedAccount);
                await Load();
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            }
        }
        private async void On_Add(object parameter)
        {
            string tempVal = (string)parameter;
            if (!IsAlreadyIn())
            {
                if (!string.IsNullOrEmpty(tempVal))
                {
                    Account account = new Account(SelectedDate, double.Parse(Money.ToString()));
                    await accountRepository.SaveOrUpdateAsync(account);
                    Money = 0;
                    await Load();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Item already present", "Cancel");
            }
            
        }
        private async Task Load()
        {
            Accounts.Clear();
            var data = await accountRepository.GetAllAsync();
            for(int i = 0; i < data.Count; i++)
            {
                Accounts.Add(data[i]);
            }
        }
        private bool IsAlreadyIn() => Accounts.FirstOrDefault(account => account.DateTime.ToString("M").Contains(SelectedDate.ToString("M"))) != null;

    }
}
