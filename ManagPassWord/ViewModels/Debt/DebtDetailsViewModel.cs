using ManagPassWord.Models;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public class DebtDetailsViewModel:BaseViewModel, IQueryAttributable
    {
        public static event Action<DebtModel> OnUiUpdate;
        private DebtModel debt = new();
        public DebtModel DebtDetails
        {
            get => debt;
            set => UpdateObservable(ref debt, value);
        }
        public ICommand EditCommand { get; private set; }
        public DebtDetailsViewModel()
        {
            EditCommand = new Command(OnEdit);
            DebtModel.OnUiUpdate += (sender) =>
            {
                DebtDetails = sender;
                OnOnUpdate(sender);
            };
        }
        private async void OnEdit(object sender)
        {
            string title = (string)sender;
            string result = await Shell.Current.DisplayPromptAsync(title, "Update");
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
