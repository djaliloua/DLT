using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MVVM;
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
        private MaxMin _maxSaleValue;
        public MaxMin MaxSaleValue
        {
            get => _maxSaleValue;
            set => UpdateObservable(ref _maxSaleValue, value);
        }
        private MaxMin _minSaleValue;
        public MaxMin MinSaleValue
        {
            get => _minSaleValue;
            set => UpdateObservable(ref _minSaleValue, value);
        }
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
            Show = true;
            accountRepository = _accountRepository;
            Accounts = new ObservableCollection<Account>();
            SelectedDate = DateTime.Now;
            _ = Load();
            _ = GetMax()
                .ContinueWith(t => 
                MakeSnackBarAsync($"Best day: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value} CFA")
                );
            Show = false;
            AddCommand = new Command(On_Add);
            DeleteCommand = new Command(On_Delete);
        }
        private async void On_Delete(object parameter)
        {
            if (CanProceed)
            {
                if(await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    await accountRepository.DeleteAsync(SelectedAccount);
                    await Load();
                }
                
            }
            else
            {
                await Shell.Current.DisplayAlert("Message", "Please select the item first", "Cancel");
            }
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
        private async void GetMin()
        {
            MaxMin min = new();
            IList<MaxMin> val = await accountRepository.GetMinAsync();
            if (val.Count == 1)
            {
                min = val[0];
            }
            MinSaleValue = min;
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
            var snackbar = Snackbar.Make(text, duration:duration, visualOptions: snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }
        public async Task Load()
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
