using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MVVM;
using PurchaseManagement.MVVM.Models.Accounts;
using PurchaseManagement.MVVM.Models.DTOs;
using PurchaseManagement.ServiceLocator;
using PurchaseManagement.DataAccessLayer.Repository;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class City
    {
        public string Name { get; set; }
        public double Population { get; set; }
    }
    public class AccountAnalyticViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        public ISeries[] LineSeries { get; set; } =
    {
        new LineSeries<AccountDTO>
        {
            Values = ViewModelLocator.AccountListViewViewModel.GetItems(),
            DataLabelsFormatter = point => $"{point.Model?.Money} CFA",
            DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
            XToolTipLabelFormatter = point => $"{point.Model?.DateTime:D}",
            DataLabelsPosition = DataLabelsPosition.Top,
            Fill = null,
            Mapping = (point, index) => new(point.Money/10000, index)  
        }
    };
        public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "Chaki Shop",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };
        
        public static ObservableCollection<Statistics> Statistics { get; private set; }

        public ISeries[] BarSeries { get; set; }
        public ColumnSeries<Statistics> col;


        public AccountAnalyticViewModel(IAccountRepository _accountRepository)
        {
            ShowActivity();
            accountRepository = _accountRepository;
            Statistics = new ObservableCollection<Statistics>();
            _ = Load();
            //DateTime.Now.ToString("")
            var paints = new SolidColorPaint[]
        {
            new(SKColors.Red),
            new(SKColors.Green),
            new(SKColors.Blue),
            new(SKColors.Yellow)
        };
            col = new ColumnSeries<Statistics>
            {
                Values = Statistics,
                DataLabelsFormatter = point => $"{point.Model?.Day}",
                DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
                DataLabelsPosition = DataLabelsPosition.Top,
                XToolTipLabelFormatter = point => $"{point.Model?.CountMoney}",
                // Defines the distance between every bars in the series
                Padding = 5,
                Rx = 50,
                Ry = 50,
                IgnoresBarPosition = true,
                // Defines the max width a bar can have
                MaxBarWidth = double.PositiveInfinity,
                //Mapping
                Mapping = (stat, index) => new(stat.AvgMoney/1000, index)
            };
            
            BarSeries = new ISeries[] { col };
            HideActivity();
        }


        

        public async Task Load()
        {
            Statistics.Clear();
            var data = await accountRepository.GetStatisticsAsync();
            for (int i = 0; i < data.Count; i++)
            {
                Statistics.Add(data[i]);
            }
        }
    }
}
