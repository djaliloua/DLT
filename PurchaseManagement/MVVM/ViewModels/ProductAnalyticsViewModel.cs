using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore;
using MVVM;
using PurchaseManagement.MVVM.Models.DTOs;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Windows.Input;
using Microsoft.Maui.Handlers;
using PurchaseManagement.NavigationLib.Abstractions;
using PurchaseManagement.NavigationLib.Models;

namespace PurchaseManagement.MVVM.ViewModels
{
    public record Summary(string Name, int Count, double TotalPrice, double TotalQuantity);
    public class HelperClass
    {
        string[] labels = [];
        double[] values = [];
        ObservableCollection<Summary> summaryCollection = new ObservableCollection<Summary>();
        public ObservableCollection<Summary> GetSummary()
        {
            return summaryCollection; 
        }
        public string[] GetLabels()
        {
            return labels;
        }
        public double[] GeValues()
        {
            return values;
        }
        public void Load(List<ProductDto> Items)
        {
            var GB = Items.GroupBy(item => item.IsPurchased).Select(item => new Summary(item.Key == true ? "Bought" : "Not Bought", item.Count(), 
                item.Sum(x => x.Item_Price), item.Sum(x => x.Item_Quantity)));
            
            summaryCollection = GB.ToObservableCollection();    
            var lab = new List<string>();
            var val = new List<double>();
            foreach (var item in GB)
            {
                lab.Add(item.Name);
                val.Add(item.TotalPrice);
                
            }
            labels = lab.ToArray();
            values = val.ToArray();
        }
        
    }
    public class ProductAnalyticsViewModel : BaseViewModel, INavigatedEvents
    {
        private readonly INavigationService navigationService;
        private ObservableCollection<ProductDto> _products;
        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set
            {
                _products = value;
                HelperClass helper = new();
                helper.Load(Products.ToList());
                SetValues(helper);
                Summary = helper.GetSummary();
                SetDate();
            }
        }
        private ObservableCollection<Summary> _summary;
        public ObservableCollection<Summary> Summary
        {
            get => _summary;
            set => UpdateObservable(ref _summary, value);  
        }
        private string _date;
        public string Date
        {
            get => _date;
            set => UpdateObservable(ref _date, value);  
        }
        private IEnumerable<ISeries> _series;
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set => UpdateObservable(ref _series, value);    
        }
        public ICommand BackButtonCommand { get; private set; }
        public ProductAnalyticsViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            BackButtonCommand = new Command(OnBackButtonCommand);
        }
        private async void OnBackButtonCommand(object parameter)
        {
            await navigationService.Navigate("..");
        }
        private void SetValues(HelperClass helper)
        {
            string[] _names = helper.GetLabels();
            int _index = 0;
            Series = helper.GeValues().AsPieSeries((value, series) =>
            {
                series.Name = _names[_index++ % _names.Length];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                series.DataLabelsSize = 15;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsFormatter =
                   point =>
                       $"{point.Coordinate.PrimaryValue}";
                series.ToolTipLabelFormatter = point => $"{point.StackedValue!.Share:P2}";
            });
        }
        private void SetDate()
        {
            if(Products.Count > 0)
            {
                Date = Products[0].Purchase?.PurchaseDate;
            }
        }
        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            await Task.Delay(1);
            Products = parameters.GetValue<ObservableCollection<ProductDto>>("product");
        }

        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.Delay(1);
        }
    }
}

