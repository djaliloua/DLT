﻿using FluentValidation.Results;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Validations;
using PurchaseManagement.Commons.Notifications.Implementations;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement.Utilities
{
    public static class MarketFormViewModelUtility
    {
        private static readonly INotification _toastNotification;
        private static readonly IPurchaseRepositoryApi _genericRepositoryApi;
        private static ProductValidation productValidation = new();
        static MarketFormViewModelUtility()
        {
            _genericRepositoryApi = ViewModelLocator.GetService<IPurchaseRepositoryApi>();
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
                    purchase = new PurchaseDto("test", ViewModelLocator.MainViewModel.SelectedDate);
                    purchase.Add(product);
                }
                var count = await ViewModelUtility.SaveAndUpdateUI(purchase);
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
                purchase.UpdateStatistics();
                await ViewModelUtility.SaveAndUpdateUI(purchase);
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
