using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.Pages;
using PurchaseManagement.DataAccessLayer;

using SkiaSharp.Views.Maui.Controls.Hosting;
using UraniumUI;
using Microsoft.Maui.LifecycleEvents;
using System.Diagnostics;
using PurchaseManagement.ServiceLocator;

namespace PurchaseManagement
{
    public static class MauiProgram
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            mauiAppBuilder.Services.AddScoped<AccountAnalyticPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder DbContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IRepository, Repository>();
            mauiAppBuilder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<AboutViewModel>();
            mauiAppBuilder.Services.AddScoped<SettingsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AccountAnalyticViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountViewModel>();
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddTransient<MarketFormViewModel>();
            return mauiAppBuilder;
        }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .PagesExtensions()
                .DbContextExtension()
                .ViewModelsExtension()
                .ConfigureMopups()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseSkiaSharp(true)
                .UseUraniumUIMaterial()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                     events.AddAndroid(android => android
                    .OnStart(async (activity) => await ViewModelLocator.MainViewModel.Load()));
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddMaterialIconFonts();
                });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}