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
        private DebtModel _debt;
        public DebtModel Debt
        {
            get => _debt;
            set => UpdateObservable(ref _debt, value);
        }
        public ICommand SaveCommand { get; private set; }
        public DebtFormViewModel(IRepository<DebtModel> db)
        {
            Debt = new DebtModel();
            SaveCommand = new Command(On_Save);
            _db = db;
        }
        private async void On_Save(object sender)
        {
            await _db.SaveItemAsync(Debt);
            await ViewModelLocator.DebtPageViewModel.Load();
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                Debt = query["debt"] as DebtModel;
            }
        }
    }
}
