using CommunityToolkit.Mvvm.Messaging;
using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;
using Mapster;
using Patterns;
using Patterns.Abstractions;
using Patterns.Implementations;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Debt
{

   
    public class LoadDebtService : ILoadService<DebtModelDTO>
    {
        public IList<DebtModelDTO> Reorder(IList<DebtModelDTO> items)
        {
            return items.OrderByDescending(x => x.DebtDate).ToList();
        }
    }
    public class DebtPageViewModel : Loadable<DebtModelDTO>, IQueryAttributable
    {
        private readonly IGenericRepository<DebtModel> _debtRepository;

        #region Properties
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
        #endregion

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand GoSearchCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        #endregion

        #region Constructor
        public DebtPageViewModel(IGenericRepository<DebtModel> db, 
            ILoadService<DebtModelDTO> loadService):base(loadService)
        {
            _debtRepository = db;
            load();
            CommandSetups();
            RegisterMessages();
        }
        #endregion

        #region Private methods
        private void RegisterMessages()
        {
            WeakReferenceMessenger.Default.Register<DebtModelDTO, string>(this, "update", (sender, arg) =>
            {
                SaveOrUpdateItem(arg);
            }
            );
        }
        private void CommandSetups()
        {
            AddCommand = new Command(OnAdd);
            OpenCommand = new Command(OnOpen);
            GoSearchCommand = new Command(OnGoSearch);
            SettingCommand = new Command(OnSetting);
            AboutCommand = new Command(OnAbout);
        }
        #endregion

        #region Handlers
        private async void OnAbout(object sender)
        {
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }
        private async void OnSetting(object sender)
        {
            await Shell.Current.GoToAsync(nameof(DebtSettingPage));
        }
        private async void OnGoSearch(object sender)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
        private async void OnOpen(object sender)
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
        private async void OnAdd(object sender)
        {
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "debt", new DebtModelDTO() }
                        };
            await Shell.Current.GoToAsync(nameof(DebtFormPage), navigationParameter);
        }
        #endregion
        private async void load()
        {
            ShowActivity();
            var repo = await _debtRepository.GetAllItemsAsync();
            var data = repo.Adapt<List<DebtModelDTO>>();
            await Task.Run(async() => await LoadItems(data));
            HideActivity();
        }
        
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {

            }
        }

        
    }
}
