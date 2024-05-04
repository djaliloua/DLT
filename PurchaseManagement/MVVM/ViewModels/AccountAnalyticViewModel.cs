using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MVVM;
using PurchaseManagement.MVVM.Models;
using PurchaseManagement.DataAccessLayer;
using PurchaseManagement.Services;
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
        new LineSeries<Account>
        {
            Values = ViewModelLocator.AccountViewModel.Accounts,
            DataLabelsFormatter = point => $"{point.Model?.Money} CFA",
            DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30)),
            TooltipLabelFormatter = point => $"{point.Model?.DateTime:D}",
            DataLabelsPosition = DataLabelsPosition.Top,
            Fill = null,
            Mapping = (point, index) =>
                {
                    index.PrimaryValue = point.Money/10000;
                    index.SecondaryValue = index.Context.Index;
                }
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
                TooltipLabelFormatter = point => $"{point.Model?.CountMoney}",
                // Defines the distance between every bars in the series
                Padding = 5,
                Rx = 50,
                Ry = 50,
                IgnoresBarPosition = true,
                // Defines the max width a bar can have
                MaxBarWidth = double.PositiveInfinity,
                //Mapping
                Mapping = (stat, index) =>
                {
                    index.PrimaryValue = stat.AvgMoney / 10000;
                    index.SecondaryValue = index.Context.Index;
                }
            };
            
            BarSeries = new ISeries[] { col};
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
