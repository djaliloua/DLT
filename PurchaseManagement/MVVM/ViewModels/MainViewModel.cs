using Mopups.Services;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public ObservableCollection<Purchases> Purchases { get; }
        
        private Purchases _selectedPurchase;
        public Purchases SelectedPurchase
        {
            get => _selectedPurchase;
            set => UpdateObservable(ref _selectedPurchase, value, async () =>
            {
                if(value != null)
                {
                    Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "purchase", SelectedPurchase }
                        };
                    await Shell.Current.GoToAsync(nameof(PurchaseItemsPage), navigationParameter);
                }
            });
        }
        private readonly IRepository _db;
        public ICommand AddCommand { get; private set; }
        public MainViewModel(IRepository db)
        {
            _db = db;
            Purchases = new ObservableCollection<Purchases>();
            Load();
            AddCommand = new Command(On_Add);
        }
        private async void On_Add(object sender)
        {
            await MopupService.Instance.PushAsync(new PurchaseItemForm());
            //string result = await Shell.Current.DisplayPromptAsync("Add", "Name", "OK");
            //if (!string.IsNullOrEmpty(result))
            //{
            //    await _db.SavePurchaseAsync(new Models.Purchases(result));
            //}
        }
        private async void Load()
        {
            if (_db != null)
            {
                await LoadPurchasesAsync();
            }
        }
        public async Task LoadPurchasesAsync()
        {
            Purchases.Clear();
            IEnumerable<Purchases> _purchases = await _db.GetAllPurchases();
            foreach (Purchases purchase in _purchases)
            {
                Purchases.Add(purchase);
            }
        }
    }
}
