using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Diagnostics;
using Font = Microsoft.Maui.Font;

namespace PurchaseManagement.Commons
{
    public interface INotication
    {
        void ShowNotification(string message);
    }
    public class ToastNotification : INotication
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ToastDuration duration;
        double textSize;
        public ToastNotification():this(ToastDuration.Short, 14)
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
    public class SnackBarNotification : INotication
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public async void ShowNotification(string message)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.Red,
                TextColor = Colors.Green,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };

            string text = message;
            string actionButtonText = "Click Here to Dismiss";
            Action action = async () => await Shell.Current.DisplayAlert("Snackbar ActionButton Tapped", "The user has tapped the Snackbar ActionButton", "OK");
            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
    public class TestDecorator : INotication
    {
        private readonly INotication _notication;
        public TestDecorator(INotication notication)
        {
            _notication = notication;
        }
        public void ShowNotification(string message)
        {
            Debug.WriteLine(message);
            _notication.ShowNotification(message);
        }
    }
}
