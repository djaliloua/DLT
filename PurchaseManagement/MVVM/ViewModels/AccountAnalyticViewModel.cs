using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MVVM;
using PurchaseManagement.DataAccessLayer;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AccountAnalyticViewModel:BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private MaxMin _maxSaleValue;
        public MaxMin MaxSaleValue
        {
            get => _maxSaleValue;
            set => UpdateObservable(ref _maxSaleValue, value);
        }
        public ObservableCollection<Statistics> Statistics { get;}
        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 2 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 1 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 3 } }
            };
        public AccountAnalyticViewModel(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
            Statistics = new ObservableCollection<Statistics>();
            _ = Load();
            GetMax();
        }

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "My chart title",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
        private async void GetMax()
        {
            MaxMin max = new();
            IList<MaxMin> val = await accountRepository.GetMaxAsync();
            if(val.Count == 1)
            {
                max = val[0];
            }

            MaxSaleValue = max;
        }
        private async Task<MaxMin> GetMin()
        {
            MaxMin min = new();
            IList<MaxMin> val = await accountRepository.GetMinAsync();
            if (val.Count == 1)
            {
                min = val[0];
            }
            return min;
        }

        public async Task Load()
        {
            Statistics.Clear();
            var data = await accountRepository.GetStatisticsAsync();
            for(int i=0; i <data.Count; i++)
            {
                Statistics.Add(data[i]);
            }
        }
    }
}
