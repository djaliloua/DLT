using AutoMapper;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AccountViewModel: Loadable<AccountDTO>
    {
        private readonly IAccountRepository accountRepository;
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
        private Mapper mapper;
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public AccountViewModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
            mapper = MapperConfig.InitializeAutomapper();
            SelectedDate = DateTime.Now;
            _ = LoadItems();
            _ = GetMax()
                .ContinueWith(async (t) =>
                {
                    if(Items.Count > 0)
                    {
                        await MakeSnackBarAsync($"Best day: {MaxSaleValue.DateTime:M}, {MaxSaleValue.Value} CFA");
                    }
                }
                );
            AddCommand = new Command(On_Add);
            DeleteCommand = new Command(On_Delete);
        }
        public override void Reorder()
        {
            var data = Items.OrderByDescending(a => a.DateTime).ToList();
            SetItems(data);
        }
        public override async Task LoadItems()
        {
            ShowProgressBar();
            var data = await accountRepository.GetAllAsync();
            var dt = data.Select(mapper.Map<AccountDTO>).ToList();
            SetItems(dt);
            
            HideProgressBar();
        }
        protected override void OnShow()
        {
            IsSavebtnEnabled = !Show;
        }
        private async void On_Delete(object parameter)
        {
            if (IsSelected)
            {
                if(await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
                {
                    await accountRepository.DeleteAsync(mapper.Map<Account>(SelectedItem));
                    DeleteItem(SelectedItem);
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
        private async void On_Add(object parameter)
        {
            string tempVal = (string)parameter;
            if (!IsAlreadyIn())
            {
                if (!string.IsNullOrEmpty(tempVal))
                {
                    Account account = new Account(SelectedDate, double.Parse(Money.ToString()));
                    var x = await accountRepository.SaveOrUpdateAsync(account);
                    Money = 0;
                    AddItem(mapper.Map<AccountDTO>(x));
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
        private bool IsAlreadyIn() => Items.FirstOrDefault(account => $"{account.DateTime:yyyy-MM-dd}".Contains($"{SelectedDate:yyyy-MM-dd}")) != null;

        
    }
}
