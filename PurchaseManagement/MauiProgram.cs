using PurchaseManagement.MVVM.ViewModels;
using PurchaseManagement.Pages;
using CommunityToolkit.Maui;
using UraniumUI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.Logging;
using PurchaseManagement.MVVM.ViewModels.AccountPage;
using PurchaseManagement.DataAccessLayer.Repository;
using PurchaseManagement.MVVM.Models.MarketModels;
using MarketModels = PurchaseManagement.MVVM.Models.MarketModels;
using PurchaseManagement.Commons;
using PurchaseManagement.MVVM.Models.DTOs;
using Mapster;
using CommunityToolkit.Maui.Storage;
using PurchaseManagement.NavigationLib.Services;
using PurchaseManagement.NavigationLib.Abstractions;


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
            mauiAppBuilder.Services.AddScoped<INavigationService, NavigationService>();
            mauiAppBuilder.Services.AddScoped<ProductAnalytics>();
            mauiAppBuilder.Services.AddScoped<ProductsPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder DbContextExtension(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            mauiAppBuilder.Services.AddSingleton<IProductRepository, ProductRepository>();
            mauiAppBuilder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<MarketModels.Location>, LocationRepository>();
            mauiAppBuilder.Services.AddSingleton<IGenericRepository<PurchaseStatistics>, StatisticsRepository>();
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
            mauiAppBuilder.Services.AddScoped<ProductAnalyticsViewModel>();
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