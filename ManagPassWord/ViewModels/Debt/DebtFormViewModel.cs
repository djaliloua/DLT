using AutoMapper;
using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.ServiceLocators;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public class DebtFormViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository<DebtModel> _db;
        private readonly Mapper mapper = MapperConfig.InitializeAutomapper();
        private DebtModelDTO _debt;
        public DebtModelDTO Debt
        {
            get => _debt;
            set => UpdateObservable(ref _debt, value);
        }
        public ICommand SaveCommand { get; private set; }
        public DebtFormViewModel(IRepository<DebtModel> db)
        {
            Debt = new DebtModelDTO();
            SaveCommand = new Command(On_Save);
            _db = db;
        }
        private async void On_Save(object sender)
        {
            DebtModel debtitem = await _db.SaveItemAsync(mapper.Map<DebtModel>(Debt));
            ViewModelLocator.DebtPageViewModel.AddOrUpdateItem(mapper.Map<DebtModelDTO>(debtitem));
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                Debt = query["debt"] as DebtModelDTO;
            }
        }
    }
}
