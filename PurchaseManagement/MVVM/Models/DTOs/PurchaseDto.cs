﻿

using MVVM;
using PurchaseManagement.Utilities;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.Models.DTOs
{
    public class PurchaseDto:BaseViewModel
    {
        private int _purchase_Id;
        public int Id
        {
            get => _purchase_Id;
            set => UpdateObservable(ref _purchase_Id, value);
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => UpdateObservable(ref _title, value);
        }
        private string _purchase_Date;
        public string PurchaseDate
        {
            get => _purchase_Date;
            set => UpdateObservable(ref _purchase_Date, value);
        }
        private ObservableCollection<ProductDto> _purchase_Items = new ObservableCollection<ProductDto>();
        public ObservableCollection<ProductDto> Products
        {
            get => _purchase_Items;
            set => UpdateObservable(ref _purchase_Items, value);
        }

        private ProductStatisticsDto _purchaseSatistics;
        public ProductStatisticsDto ProductStatistics
        {
            get => _purchaseSatistics;
            set => UpdateObservable(ref _purchaseSatistics, value);
        }
        public PurchaseDto(string title, DateTime dt)
        {
            Title = title;
            PurchaseDate = dt.ToString("yyyy-MM-dd");
        }
        public PurchaseDto()
        {
            
        }
        public void Update(ProductDto product)
        {
            if (product == null) return;
            for(int i = 0; i < Products.Count; i++)
            {
                if(Products[i].Id == product.Id)
                {
                    Products[i] = product;
                    return;
                }
            }
        }
        public void UpdateStatistics()
        {
            PurchaseUtility.UpdateStatistics(this);
        }
        public void Add(ProductDto product)
        {
            Products.Add(product);
            PurchaseUtility.UpdateStatistics(this);
        }
        public void Remove(ProductDto product)
        {
            ProductDto p = Products.FirstOrDefault(p => p.Id == product.Id);
            int index = Products.IndexOf(p);
            if (index >= 0)
            {
                Products.RemoveAt(index);
            }
            PurchaseUtility.UpdateStatistics(this);
        }
    }
}
