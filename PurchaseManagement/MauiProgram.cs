using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.Pages;
using CommunityToolkit.Maui;
using UraniumUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.DTOs;
using Mapster;
using CommunityToolkit.Maui.Storage;


namespace PurchaseManagement
{
    public static class MauiProgram
    {
        public static MauiAppBuilder PagesExtensions(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetails>();
            mauiAppBuilder.Services.AddScoped<AccountAnalyticPage>();
            mauiAppBuilder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            mauiAppBuilder.Services.AddScoped<ExportContext<AccountDTO>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<AccountDTO>, AccountTxtStrategy>();
            mauiAppBuilder.Services.AddScoped<ExportContext<ProductDto>>();
            mauiAppBuilder.Services.AddScoped<IExportStrategy<ProductDto>, ProductTxtStrategy>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder DbContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            mauiAppBuilder.Services.AddSingleton<IProductRepository, ProductRepository>();
            mauiAppBuilder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<MarketModels.Location>, LocationRepository>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<PurchaseStatistics>, StatisticsRepository>();
            //mauiAppBuilder.Services.AddScoped<INotification, ToastNotification>();
            //mauiAppBuilder.Services.AddScoped<INotification, SnackBarNotification>();
            mauiAppBuilder.Services.AddMemoryCache();
            mauiAppBuilder.Services.AddMapster();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder ViewModelsExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddScoped<AboutViewModel>();
            mauiAppBuilder.Services.AddScoped<SettingsViewModel>();
            mauiAppBuilder.Services.AddSingleton<AccountAnalyticViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountListViewViewModel>();
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            mauiAppBuilder.Services.AddSingleton<ProductItemsViewModel>();
            mauiAppBuilder.Services.AddSingleton<PurchaseItemDetailsViewModel>();
            mauiAppBuilder.Services.AddTransient<MarketFormViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountPageViewModel>();
            mauiAppBuilder.Services.AddScoped<AccountHeaderViewModel>();
            return mauiAppBuilder;
        }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .PagesExtensions()
                .DbContextExtension()
                .ViewModelsExtension()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseSkiaSharp(true)
                .UseUraniumUIMaterial()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    // events.AddAndroid(android => android
                    //.OnStart(async (activity) => await ViewModelLocator.MainViewModel.Load()));
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddMaterialIconFonts();
                })
                .ConfigureMauiHandlers(handler =>
                {
#if ANDROID
handler.AddHandler<Shell, TabBarBadgeRender>();
#endif
                })
                ;

#if DEBUG
            builder.Logging.AddDebug();
#endif
            AppCenter.Start("android=98c12bc0-8ce8-4fd0-a772-8ed0fa474323;" +
                  "windowsdesktop=ef285771-ff13-4590-91c2-8b78ffa2ffeb;" +
                  "ios={};" +
                  "macos={Your macOS App secret here};",
                  typeof(Analytics), typeof(Crashes));
            return builder.Build();
        }
    }
}