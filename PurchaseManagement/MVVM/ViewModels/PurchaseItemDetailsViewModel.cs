using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using MauiNavigationHelper.NavigationLib.Abstractions;
using MauiNavigationHelper.NavigationLib.Models;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemDetailsViewModel:BaseViewModel, INavigatedEvents
    {
        private readonly INavigationService _navigationService;
        private ProductDto _purchaseDetails;
        public ProductDto PurchaseDetails
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
            PurchaseDetails = parameters.GetValue<ProductDto>("details");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFrom(NavigationParameters parameters)
        {
            //ViewModelLocator.ProductItemsViewModel.ResetSelectedItem();
            return Task.CompletedTask;
        }
    }
}
