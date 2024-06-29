using MVVM;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels.AccountPage
{
    public class AccountHeaderViewModel:BaseViewModel
    {
        #region Private methods
        private readonly IGenericRepository<Person> _personRepository;
        private readonly IGenericRepository<Car> _carRepository;
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
        public AccountHeaderViewModel(IGenericRepository<Person> personRepository, IGenericRepository<Car> carRepository)
        {
            _personRepository = personRepository;
            _carRepository = carRepository;
            Init();
            CommandSetup();
        }
        #endregion

        #region Private Methods

        private void CommandSetup()
        {
            AddCommand = new Command(OnAdd);
            
        }
        private async void Init()
        {
            SelectedDate = DateTime.Now;
            IsSavebtnEnabled = true;
            await Seed();
        }

        #endregion

        #region Handlers
        
        private void OnAdd(object parameter)
        {
            ViewModelLocator.AccountListViewViewModel.AddItem(new AccountDTO(SelectedDate, Money));
            Money = 0;
        }
        #endregion
        // Data Seeding
        private async Task Seed()
        {
            Person person;
            for(int i=0; i < 10; i++)
            {
                person = await _personRepository.SaveOrUpdateItem(new($"Name {i}"));
                await _carRepository.SaveOrUpdateItem(new(person.Id, person.Name));
            }
        }
    }
}
