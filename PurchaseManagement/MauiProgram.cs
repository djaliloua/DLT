using CommunityToolkit.Maui;
using UraniumUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.Logging;
using MauiNavigationHelper.NavigationLib.Services;
using PurchaseManagement.ExtensionMethods;


namespace PurchaseManagement
{
    public static class MauiProgram
    {
        
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .PagesExtensions()
                .UtilityExtension()
                .RepositoryExtension()
                .ViewModelsExtension()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseSkiaSharp(true)
                .UseUraniumUIMaterial()
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