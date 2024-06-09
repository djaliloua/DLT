using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace PurchaseManagement.Commons
{
    public class ToastNotification : INotification
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ToastDuration duration;
        double textSize;
        public ToastNotification() : this(ToastDuration.Short, 14)
        {

        }
        public ToastNotification(ToastDuration duration, double textSize)
        {
            this.duration = duration;
            this.textSize = textSize;
        }
        public async void ShowNotification(string message)
        {
            string text = $"{message} ";
            var toast = Toast.Make(text, duration, textSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
