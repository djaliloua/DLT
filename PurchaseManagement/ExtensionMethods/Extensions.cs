﻿using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.Pages;
using PurchaseManagement.DataAccessLayer;

namespace PurchaseManagement.ExtensionMethods
{
    public static class Extensions
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            //mauiAppBuilder.Services.AddSingleton<>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder DbContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IRepository, Repository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();//PurchaseItemsViewModel
            mauiAppBuilder.Services.AddSingleton<PurchaseItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseFormViewModel>();
            //mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            return mauiAppBuilder;
        }
    }
}
