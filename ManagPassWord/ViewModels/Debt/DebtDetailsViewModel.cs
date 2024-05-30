using AutoMapper;
using ManagPassWord.Data_AcessLayer;
using ManagPassWord.Models;
using ManagPassWord.ServiceLocators;
using MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public interface IPublisher<TItem>
    {
        event Action<TItem> Published;
        void OnPublished(TItem item);
    }
    public class Publisher<TItem> : IPublisher<TItem>
    {
        public event Action<TItem> Published;

        public void OnPublished(TItem item)
        {
            Published?.Invoke(item);
        }
    }
    public interface ISubscriber<IItem>
    {
        void Subscribe(IItem obj);
    }
    public class Subscriber<TItem> : ISubscriber<TItem>
    {
        private readonly IPublisher<TItem> _publisher;
        public Subscriber(IPublisher<TItem> publisher)
        {
            _publisher = publisher;
            _publisher.Published += _publisher_Published;
        }

        private void _publisher_Published(TItem obj)
        {
            Subscribe(obj);
        }

        public void Subscribe(TItem obj)
        {
            throw new NotImplementedException();
        }
    }
    public class DebtDetailsViewModel:BaseViewModel, IQueryAttributable
    {
        private readonly IRepository<DebtModel> _db;
        private readonly Mapper mapper = MapperConfig.InitializeAutomapper();
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
        public DebtDetailsViewModel(IRepository<DebtModel> db)
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
        private async void On_Save(object sender)
        {
            
            if(DebtDetails != null)
            {
                if (await _db.SaveItemAsync(mapper.Map<DebtModel>(DebtDetails)) is DebtModel debt && debt.Id != 0)
                {
                    MessagingCenter.Send(this, "update", debt);
                    
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
                    await _db.DeleteById(mapper.Map<DebtModel>(DebtDetails));
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
