using Android.App;
using Android.Content;
using Android.Graphics;
using Plugin.FirebasePushNotifications;
using Plugin.FirebasePushNotifications.Platforms;
using Plugin.FirebasePushNotifications.Platforms.Channels;
using And = Android.App;
using Core = AndroidX.Core.App;

namespace PurchaseManagement.Platforms.Android.Notifications
{
    
    public static class DisplayNotification
    {
        public static void ShowLocalNotification(string title, string body, Bitmap bigPicture)
        {
            string GROUP_KEY_WORK_EMAIL = "com.android.example.WORK_EMAIL";
            Core.Person person = new Core.Person
                .Builder()
                .SetName("ChakiShop")
                .Build()
                ;
            Core.NotificationCompat.MessagingStyle messagingStyle = new Core.NotificationCompat.MessagingStyle(person)
                .SetConversationTitle("ChakiShop")
                .AddMessage(body, Java.Lang.JavaSystem.CurrentTimeMillis(), person);

            var channelId = "chakishop-chnl-id";
            var context = And.Application.Context;
            var notificationManager = Core.NotificationManagerCompat.From(context);
            var style = bigPicture != null
                ? new Core.NotificationCompat.BigPictureStyle()
                    .BigPicture(bigPicture)
                    .SetSummaryText(body)
                : null;

            var builder = new Core.NotificationCompat.Builder(context, channelId)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetSmallIcon(Resource.Drawable.abc_cab_background_top_material) // Replace with your app's icon
                //.SetGroup(GROUP_KEY_WORK_EMAIL)
                .SetAutoCancel(true);

            if (style != null)
                builder.SetStyle(messagingStyle);
            else
                builder.SetStyle(messagingStyle);

            notificationManager.Notify(1001, builder.Build());
        }
        
    }

    public static class NotificationChannelSamples
    {
        public static IEnumerable<NotificationChannelRequest> GetAll()
        {
            yield return new NotificationChannelRequest
            {
                ChannelId = "chakishop-chnl-id",
                ChannelName = "Shop-Channel",
                Description = "ChakiShop notfication",
                LockscreenVisibility = NotificationVisibility.Public,
                Importance = NotificationImportance.High,
                IsDefault = true,
            };
        }

    }
}
