using FluentValidation.Results;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Validations;
using PurchaseManagement.Commons.Notifications.Implementations;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ExtensionMethods;

namespace PurchaseManagement.Utilities
{
    public static class MarketFormViewModelUtility
    {
        private static readonly INotification _toastNotification;
        private static ProductValidation productValidation = new();
        static MarketFormViewModelUtility()
        {
            _toastNotification = new ToastNotification();
        }
        public static async Task<bool> CreateAndAddProduct(ProductDto product)
        {
            ViewModelLocator.MarketFormViewModel.ShowActivity();
            bool result = false;
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                if (ViewModelLocator.MainViewModel.GetItemByDate() is PurchaseDto purchase)
                {
                    purchase.Add(product);
                }
                else
                {
                    purchase = new PurchaseDto("test", ViewModelLocator.MarketFormViewModel.SelectedDate);
                    purchase.Add(product);
                }
                var count = await ViewModelUtility.SaveAndUpdateUI(purchase.FromDto());
                await _toastNotification.ShowNotification($"{count}");
                result = true;

            }
            else
            {
                result = false;
                await _toastNotification.ShowNotification(validationResult.Errors[0].ErrorMessage);

            }
            ViewModelLocator.MarketFormViewModel.HideActivity();
            return result;
        }
       
        public static async Task<bool> UpdateProductItem(PurchaseDto purchase, ProductDto product)
        {
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                purchase.Update(product);
                purchase.UpdateStatistics();
                await ViewModelUtility.SaveAndUpdateUI(purchase.FromDto());
            }
            else
            {
                await _toastNotification.ShowNotification(validationResult.Errors[0].ErrorMessage);
                return false;
            }
            return true;

        }
        
    }
}
