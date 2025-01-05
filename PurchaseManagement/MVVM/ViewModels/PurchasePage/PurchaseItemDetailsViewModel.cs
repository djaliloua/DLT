using MVVM;
using PurchaseManagement.MVVM.Models.ViewModel;
using MauiNavigationHelper.NavigationLib.Abstractions;
using MauiNavigationHelper.NavigationLib.Models;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels.PurchasePage
{
    public class PurchaseItemDetailsViewModel:BaseViewModel, INavigatedEvents
    {
        private readonly INavigationService _navigationService;
        private ProductViewModel _purchaseDetails;
        public ProductViewModel PurchaseDetails
        {
            get => _purchaseDetails;
            set => UpdateObservable(ref _purchaseDetails, value);
        }
        #region Commands
        public ICommand BackCommand { get; private set; }
        #endregion
        public PurchaseItemDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new Command(OnBackCommand);
        }
        private async void OnBackCommand()
        {
            await _navigationService.Navigate("..");
        }
        public Task OnNavigatedTo(NavigationParameters parameters)
        {
            PurchaseDetails = parameters.GetValue<ProductViewModel>("details");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFrom(NavigationParameters parameters)
        {
            //ViewModelLocator.ProductItemsViewModel.ResetSelectedItem();
            return Task.CompletedTask;
        }
    }
}
