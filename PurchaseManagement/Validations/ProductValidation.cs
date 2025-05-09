﻿using FluentValidation;
using PurchaseManagement.MVVM.Models.ViewModel;

namespace PurchaseManagement.Validations
{
    public class ProductValidation:AbstractValidator<ProductViewModel>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Item_Name).NotEmpty().WithMessage("Please, add product's name");
            RuleFor(p => p.Item_Price).NotEmpty().WithMessage("Please, add product's price");  
            RuleFor(p => p.Item_Quantity).NotEmpty().WithMessage("Please, add product's quantity");
        }
    }
}
