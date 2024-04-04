using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public class DebtPageViewModel : BaseViewModel
    {
        private readonly IRepository<DebtModel> _db;
        public ObservableCollection<DebtModel> Debts { get; }
        private string _name;
        public string Name
        {
            get => _name;
            set => UpdateObservable(ref _name, value);
        }
        private string _amount;
        public string Amount
        {
            get => _amount;
            set => UpdateObservable(ref _amount, value);
        }
        private DebtModel _selectedDebt;
        public DebtModel SelectedDebt
        {
            get => _selectedDebt;
            set => UpdateObservable(ref _selectedDebt, value);
        }
        private bool CanOpen => SelectedDebt != null;
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand GoSearchCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public DebtPageViewModel(IRepository<DebtModel> db)
        {
            _db = db;
            Debts ??= new ObservableCollection<DebtModel>();
            load();
            AddCommand = new Command(On_Add);
            OpenCommand = new Command(On_Open);
            GoSearchCommand = new Command(On_GoSearch);
            SettingCommand = new Command(On_Setting);
            AboutCommand = new Command(On_About);
            DebtDetailsViewModel.OnUiUpdate += DebtDetailsViewModel_OnUiUpdate;
        }
        private async void On_About(object sender)
        {
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }
        private async void On_Setting(object sender)
        {
            await Shell.Current.GoToAsync(nameof(DebtSettingPage));
        }
        private async void On_GoSearch(object sender)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
        private void DebtDetailsViewModel_OnUiUpdate(DebtModel obj)
        {
            for(int i=0; i < Debts.Count; i++)
            {
                if(obj != null && Debts[i].Id == obj.Id)
                {
                    Debts[i] = obj; 
                    break;
                }
            }
        }

        private async void On_Open(object sender)
        {
            if (CanOpen)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "debt", SelectedDebt }
                        };
                await Shell.Current.GoToAsync(nameof(DebtDetailsPage), navigationParameter);
            }
        }
        private async void On_Add(object sender)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Amount))
            {
                await _db.SaveItemAsync(new(Name, Amount));
                await Load();
                resetValues();
            }
            else
            {
                await MessageDialogs.ShowToast("Name or Amount are empty.");
            }
        }
        private void resetValues()
        {
            Name = string.Empty;
            Amount = string.Empty;
        }
        private async void load()
        {
            await Load();
        }
        public async Task Load()
        {
            Debts.Clear();
            var repo = await _db.GetAll();
            foreach (var item in repo)
            {
                Debts.Add(item);
            }
            OnPropertyChanged(nameof(Debts));  
        }
    }
}
