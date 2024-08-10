using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MVVM;
using System.Windows.Input;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Markdown { get; } = "## Purchase Management Application(PMA)\r\n### Project description\r\nPurchase Management Application(PMA) is an application that is used to logging daily operations.\nThe app contains three main pages: Account, Purchase and Analytics pages.\r\n- Account Page: Contains estimated total daily expenses.\r\n- Analytics Page: Shows barplot of daily expenses.\r\n- Purchase Page: This page is responsible for adding products that are or will be purchased on daily basis.\r\nIt is also possible to take location where a given product has been purchased by swiping to the right on top of the product.\r\n\r\n### Technologies\r\n- MAUI\r\n- MVVM\r\n- Sqlite\r\n- Urannium\r\n- Entity framework\r\n- FingerPrint\r\n- Mapster\r\n- CommunityToolkit.Maui\r\n- CommunityToolkit.Mvvm\r\n- LiveChartsCore.SkiaSharpView.Maui\r\n- FluentValidation\r\n\r\n### Author\r\n- Software developer at MSC Technology\r\n- Abdou Djalilou Ali\r\n- djalilouagmail.com\r\n";
        public string DevName { get; set; }
        public ICommand DialCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public AboutViewModel()
        {
            DevName = "Abdou Djalilou Ali";
            //Markdown = File.ReadAllText("purchase.md");
            DialCommand = new Command(On_Dial);
            CopyCommand = new Command(On_Copy);
        }
        private async void On_Copy(object parameter)
        {
            await Clipboard.SetTextAsync(parameter.ToString());
            await MakeToast("Copied");
            
        }
        private void On_Dial(object parameter)
        {
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open(parameter.ToString());
        }
        private async Task MakeToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
