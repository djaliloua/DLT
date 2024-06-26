using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.NavigationLib.Abstractions;
using PurchaseManagement.NavigationLib.Models;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemDetailsViewModel:BaseViewModel, INavigatedEvents
    {
        private ProductDto _purchaseDetails;
        public ProductDto PurchaseDetails
        {
            get => _purchaseDetails;
            set => UpdateObservable(ref _purchaseDetails, value);
        }
        public PurchaseItemDetailsViewModel()
        {
        }
        public Task OnNavigatedTo(NavigationParameters parameters)
        {
            PurchaseDetails = parameters.GetValue<ProductDto>("details");
            return Task.CompletedTask;
        }

        public Task OnNavigatedFrom(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
