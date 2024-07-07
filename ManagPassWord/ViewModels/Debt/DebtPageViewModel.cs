using ManagPassWord.DataAcessLayer;
using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using Mapster;
using Patterns;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    
    public abstract class LoadableDebtPageViewModel<TItem> : Loadable<TItem> where TItem : DebtModelDTO
    {
        protected override void Reorder()
        {
            var data = Items.OrderByDescending(x => x.DebtDate);
            SetItems(data.ToList());
        }
    }
    public class DebtPageViewModel : LoadableDebtPageViewModel<DebtModelDTO>, IQueryAttributable
    {
        private readonly IGenericRepository<DebtModel> _debtRepository;
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

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand GoSearchCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        #endregion
        public DebtPageViewModel(IGenericRepository<DebtModel> db)
        {
            _debtRepository = db;
            load();
            CommandSetups();
            MessagingCenter.Subscribe<DebtDetailsViewModel, DebtModelDTO>(this, "update", (sender, arg) =>
            {
                SaveOrUpdateItem(arg);
            });
        }
        private void CommandSetups()
        {
            AddCommand = new Command(On_Add);
            OpenCommand = new Command(On_Open);
            GoSearchCommand = new Command(On_GoSearch);
            SettingCommand = new Command(On_Setting);
            AboutCommand = new Command(On_About);
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
        private async void On_Open(object sender)
        {
            if (IsSelected)
            {
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "debt", SelectedItem }
                        };
                await Shell.Current.GoToAsync(nameof(DebtDetailsPage), navigationParameter);
            }
        }
        private async void On_Add(object sender)
        {
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "debt", new DebtModelDTO() }
                        };
            await Shell.Current.GoToAsync(nameof(DebtFormPage), navigationParameter);
        }
        private async void load()
        {
            await LoadItems();
        }
        public override async Task LoadItems()
        {
            var repo = await _debtRepository.GetAllItemsAsync();
            var data = repo.Adapt<List<DebtModelDTO>>();
            SetItems(data);
        }
        
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {

            }
        }

        
    }
}
