using AutoMapper;
using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using Patterns;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    
    public abstract class LoadableDebtPageViewModel<TItem> : Loadable<TItem> where TItem : DebtModelDTO
    {
        public override void Reorder()
        {
            var data = Items.OrderByDescending(x => x.DebtDate);
            SetItems(data);
        }
    }
    public class DebtPageViewModel : LoadableDebtPageViewModel<DebtModelDTO>, IQueryAttributable
    {
        private readonly Mapper mapper = MapperConfig.InitializeAutomapper();
        private readonly IRepository<DebtModel> _db;
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
        public DebtPageViewModel(IRepository<DebtModel> db)
        {
            _db = db;
            load();
            CommandSetups();
            MessagingCenter.Subscribe<DebtDetailsViewModel, DebtModelDTO>(this, "update", (sender, arg) =>
            {
                AddOrUpdateItem(arg);
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
            var repo = await _db.GetAll();
            var data = repo.Select(mapper.Map<DebtModelDTO>).ToList();
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
