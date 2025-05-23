﻿using FluentValidation.Results;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Validations;
using PurchaseManagement.Commons.Notifications.Implementations;
using PurchaseManagement.MVVM.Models.ViewModel;
using Repository;
using Models.Market;

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
        public static async Task<bool> CreateAndAddProduct(ProductViewModel product)
        {
            ViewModelLocator.MarketFormViewModel.ShowActivity();
            bool result = false;
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                if (ViewModelLocator.PurchasesListViewModel.GetItemByDate() is PurchaseViewModel purchase)
                {
                    purchase.Add(product);
                }
                else
                {
                    purchase = new PurchaseViewModel("test", ViewModelLocator.MarketFormViewModel.SelectedDate);
                    purchase.Add(product);
                }
                var count = await ViewModelUtility.SaveAndUpdateUI(purchase.ToVM<PurchaseViewModel, Purchase>());
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
       
        public static async Task<bool> UpdateProductItem(PurchaseViewModel purchase, ProductViewModel product)
        {
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                purchase.Update(product);
                purchase.UpdateStatistics();
                await ViewModelUtility.SaveAndUpdateUI(purchase.ToVM<PurchaseViewModel, Purchase>());
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
