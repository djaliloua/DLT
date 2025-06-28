using CommunityToolkit.Maui;
using UraniumUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.Logging;
using MauiNavigationHelper.NavigationLib.Services;
using PurchaseManagement.ExtensionMethods;
using Mapster;


#if ANDROID
using PurchaseManagement.Platforms.Android.Notifications;
using Plugin.FirebasePushNotifications;
using Plugin.FirebasePushNotifications.Model.Queues;
using Android.App;
using Firebase;
#endif



namespace PurchaseManagement
{
    public static class MauiProgram
    {
        static MauiProgram()
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
        }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .PagesExtensions()
                .UtilityExtension()
                .ContextExtension()
                .RepositoryExtension()
                .ViewModelsExtension()
                .LoadBIExtension()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseSkiaSharp(true)
                .UseUraniumUIMaterial()
#if ANDROID
                .UseFirebasePushNotifications(option =>
                {
                    option.AutoInitEnabled = false;
                    option.QueueFactory = new PersistentQueueFactory();
                    option.Android.NotificationActivityType = typeof(MainActivity);
                    option.Android.NotificationChannels = NotificationChannelSamples.GetAll().ToArray();
                    //option.Android.FirebaseOptions = new FirebaseOptions.Builder()
                    //    .SetApplicationId("1:636930484325:android:724f2269b9d94248f2be8c") // Replace with your actual application ID
                    //    .SetApiKey("AIzaSyD")
                    //    .SetProjectId("my-notification-isolution")
                    //    .SetDatabaseUrl("https://my-notification-isolution.firebaseio.com/")
                    //    .Build();
                })
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddMaterialIconFonts();
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
handlers.AddHandler(typeof(ListView), typeof(CustomListView));
#endif
                });
            ;

#if DEBUG
            builder.Logging.AddDebug();
#endif
            
            var serviceProvider = builder.Build();
            var scope = serviceProvider.Services.CreateScope();
            ServiceResolver.RegisterScope(scope);
            
            return serviceProvider;
        }
    }
}