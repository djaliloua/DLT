using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public class DebtDetailsViewModel:BaseViewModel, IQueryAttributable
    {
        public static event Action<DebtModel> OnUiUpdate;
        private readonly IRepository<DebtModel> _db;
        private DebtModel debt = new();
        public DebtModel DebtDetails
        {
            get => debt;
            set => UpdateObservable(ref debt, value);
        }
        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public DebtDetailsViewModel(IRepository<DebtModel> db)
        {
            _db = db;
            EditCommand = new Command(OnEdit);
            SaveCommand = new Command(On_Save);
            DeleteCommand = new Command(On_Delete);
            DebtModel.OnUiUpdate += (sender) =>
            {
                DebtDetails = sender;
                OnOnUpdate(sender);
            };
        }
        private async void On_Save(object sender)
        {
            DebtModel temp = new();
            int result = 0;
            if(DebtDetails != null)
            {
                result = await _db.SaveItemAsync(DebtDetails);
                temp = DebtDetails.Clone(); 
            }
            if(result != 0)
            {
                DebtPageViewModel model = ViewModelServices.DebtPageViewModel;
                await model.Load();
                DebtDetails = temp;
                await MessageDialogs.ShowToast($"{DebtDetails.Name} has been updated");
            }
        }
        private async void On_Delete(object sender)
        {
            bool answer = await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No");
            if (answer)
            {
                if (DebtDetails.Id != 0)
                {
                    await _db.DeleteById(DebtDetails);
                    await Shell.Current.GoToAsync("..");
                    DebtPageViewModel model = ViewModelServices.DebtPageViewModel;
                    await model.Load();
                }
            }
        }
        private async void OnEdit(object sender)
        {
            string title;
            var v = (string)sender;
            string[] val = v.Split(";");
            title = val[0];
            string result = await Shell.Current.DisplayPromptAsync(title, "Update", initialValue: $"{val[1]}");
            if(result != null)
                switch (title)
                {
                    case "Name":
                        DebtDetails.Name = result;
                        break;
                    case "Amount":
                        DebtDetails.Amount = result;
                        break;
                }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                DebtDetails = query["debt"] as DebtModel;
            }
        }
        protected virtual void OnOnUpdate(DebtModel model) => OnUiUpdate?.Invoke(model);
    }
}
