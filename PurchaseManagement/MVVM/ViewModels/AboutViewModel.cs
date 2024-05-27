using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MVVM;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace PurchaseManagement.MVVM.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string DevName { get; set; }
        public ICommand DialCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public AboutViewModel()
        {
            DevName = "Abdou Djalilou Ali";
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
