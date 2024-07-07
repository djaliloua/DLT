﻿using FluentValidation.Results;
using PurchaseManagement.DataAccessLayer.Abstractions;
using PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.Commons.Notifications.Abstractions;
using PurchaseManagement.Validations;
using Mapster;
using PurchaseManagement.Commons.Notifications.Implementations;

namespace PurchaseManagement.Utilities
{
    public static class MarketFormViewModelUtility
    {
        private static readonly IPurchaseRepository _purchaseRepository;
        private static readonly INotification _toastNotification;
        private static ProductValidation productValidation = new();
        static MarketFormViewModelUtility()
        {
            _purchaseRepository = ViewModelLocator.GetService<IPurchaseRepository>();
            _toastNotification = new ToastNotification();
        }
        public static async Task<bool> CreateAndAddProduct(Product product)
        {
            ViewModelLocator.MarketFormViewModel.ShowActivity();
            bool result = false;
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                if (await _purchaseRepository.GetPurchaseByDate(ViewModelLocator.MainViewModel.SelectedDate) is Purchase purchase)
                {
                    purchase.Add(product);
                }
                else
                {
                    purchase = new Purchase("test", ViewModelLocator.MainViewModel.SelectedDate);
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
       
        public static async Task<bool> UpdateProductItem(Purchase purchase, Product product)
        {
            ValidationResult validationResult = productValidation.Validate(product);
            if (validationResult.IsValid)
            {
                ViewModelUtility.UpdateProduct(purchase, product);
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
