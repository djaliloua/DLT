using ManagPassWord.DataAcessLayer.Abstractions;
using ManagPassWord.MVVM.Models;
using ManagPassWord.ServiceLocators;
using Mapster;
using CommunityToolkit.Mvvm.Messaging;
using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Debt
{
    public class DebtDetailsViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IGenericRepository<DebtModel> _db;
        private DebtModelDTO debt = new();
        public DebtModelDTO DebtDetails
        {
            get => debt;
            set => UpdateObservable(ref debt, value);
        }
        #region Commands
        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        #endregion

        #region Constructor
        public DebtDetailsViewModel(IGenericRepository<DebtModel> db)
        {
            _db = db;
            CommandSetup();
            
        }
        #endregion

        #region Private Methods
        private void CommandSetup()
        {
            EditCommand = new Command(OnEdit);
            SaveCommand = new Command(On_Save);
            DeleteCommand = new Command(On_Delete);
        }
        private async Task<DebtModel> Update(DebtModelDTO model)
        {
            DebtModel debtModel = await _db.GetItemByIdAsync(model.Id);
            debtModel.Amount = model.Amount;
            debtModel.DebtDate = model.DebtDate;
            debtModel.PayementDate = model.PayementDate;
            debtModel.Description = model.Description;
            debtModel.Name = model.Name;
            debtModel.IsCompleted = model.IsCompleted;
            return debtModel;
        }
        private async void On_Save(object sender)
        {
            
            if (DebtDetails != null)
            {
                if (await _db.SaveOrUpdateItemAsync(await Update(DebtDetails)) is DebtModel debt && debt.Id != 0)
                {
                    WeakReferenceMessenger.Default.Send(debt, "update");
                    await MessageDialogs.ShowToast($"{DebtDetails.Name} has been updated");
                }
            }
        }
        private async void On_Delete(object sender)
        {
            if (await Shell.Current.DisplayAlert("Warning", "Do you want to delete", "Yes", "No"))
            {
                if (DebtDetails.Id != 0)
                {
                    await _db.DeleteItemAsync(DebtDetails.Adapt<DebtModel>());
                    await Shell.Current.GoToAsync("..");
                    ViewModelLocator.DebtPageViewModel.DeleteItem(DebtDetails);
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
            OnPropertyChanged(nameof(DebtDetails));
        }
        #endregion

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Count > 0)
            {
                DebtDetails = query["debt"] as DebtModelDTO;
            }
        }
        
    }
}
