#if ANDROID
using Plugin.FirebasePushNotifications.Platforms;
using PurchaseManagement.Platforms.Android.Notifications;
using A = Android.Graphics;
using Plugin.FirebasePushNotifications;
#endif

namespace PurchaseManagement
{
    public partial class App : Application
    {
        

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

#if ANDROID
        private readonly IFirebasePushNotification _firebasePushNotification;
        private readonly INotificationPermissions _permission;
        public App(IFirebasePushNotification firebasePushNotification, INotificationPermissions permission)
        {
            _firebasePushNotification = firebasePushNotification;
            _permission = permission;
            
            InitializeComponent();
            MainPage = new AppShell();
            
        }
        private async Task notificationSetup()
        {
            if (!await _permission.RequestPermissionAsync()) return;
            var categories = new[] { new NotificationCategory("market_notification", [new NotificationAction("Open", "Open", NotificationActionType.Foreground)]) };
            var token = CrossFirebasePushNotification.Current.Token;
            IFirebasePushNotification.Current.RegisterNotificationCategories(categories);

            await _firebasePushNotification.RegisterForPushNotificationsAsync();
            _firebasePushNotification.SubscribeTopic("market_notification");
            _firebasePushNotification.NotificationReceived += _firebasePushNotification_NotificationReceived;
        }

        private void _firebasePushNotification_NotificationReceived(object sender, FirebasePushNotificationDataEventArgs p)
        {
            string[] strings = p.Data["from"]?.ToString().Split("/");
            string topic = strings[strings.Length - 1];
            DisplayNotification.ShowLocalNotification(p.Data["gcm.notification.title"].ToString(),
                    p.Data["gcm.notification.body"].ToString(), null);
        }
        protected override async void OnStart()
        {
            base.OnStart();
            // Handle when your app starts
            await notificationSetup();
        }
#endif
    }
}