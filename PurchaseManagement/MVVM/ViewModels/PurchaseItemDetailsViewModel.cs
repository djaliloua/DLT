﻿using MVVM;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.MVVM.Models.DTOs;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class PurchaseItemDetailsViewModel:BaseViewModel, IQueryAttributable
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.Count > 0)
            {
                PurchaseDetails = query["details"] as ProductDto;
            }
        }
    }
}
