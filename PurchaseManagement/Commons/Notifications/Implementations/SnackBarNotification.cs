using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PurchaseManagement.Commons.Notifications.Abstractions;

namespace PurchaseManagement.Commons.Notifications.Implementations
{
    public class SnackBarNotification : INotification
    {
        public async Task ShowNotification(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Black,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                CharacterSpacing = 0.5
            };
            string text = message;
            TimeSpan duration = TimeSpan.FromSeconds(5);
            var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}
