using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Debt
{
    public class DebtFormViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IGenericRepository<DebtModel> _debtRepository;
        private DebtModelDTO _debt;
        public DebtModelDTO Debt
        {
            get => _debt;
            set => UpdateObservable(ref _debt, value);
        }
        public ICommand SaveCommand { get; private set; }
        public DebtFormViewModel(IGenericRepository<DebtModel> db)
        {
            Debt = new DebtModelDTO();
            SaveCommand = new Command(On_Save);
            _debtRepository = db;
        }
        private async void On_Save(object sender)
        {
            DebtModel debtitem = await _debtRepository.SaveOrUpdateItemAsync(Debt.Adapt<DebtModel>());
            ViewModelLocator.DebtPageViewModel.SaveOrUpdateItem(debtitem.Adapt<DebtModelDTO>());
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
